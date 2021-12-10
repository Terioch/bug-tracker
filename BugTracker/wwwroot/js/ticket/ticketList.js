
class TicketList {
    constructor() {
        if (document.getElementById("ticketListPageContainer")) {
            console.log("Initialized Ticket List");
            this.filterTicketList = this.filterTicketList.bind(this);
            this.events();
        }        
    }

    events() {
        document.getElementById("ticketListSearchInput").addEventListener("onchange", this.filterTicketList);
    }

    async filterTicketList(e) {
        const ticketListContainer = document.getElementById("ticketListContainer");
        const searchTerm = e.target.value.toLowerCase();

        try {
            const res = await fetch(`/ticket/filterTicketsReturnPartial/${searchTerm}`);
            const ticketListHTML = await res.text();
            ticketListContainer.innerHTML = ticketListHTML;
        } catch (err) {
            console.error(err);
        }
    }
}

export default TicketList;
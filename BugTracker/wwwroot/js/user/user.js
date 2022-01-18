
class User {
    constructor() {
        console.log("Initialized User");
        this.userContainer = document.getElementById("userContainer");
        this.filterProjectList = this.filterProjectList.bind(this);
        this.events();
    }

    events() {
        document.getElementById("projectListSearchInput").addEventListener("keyup", this.filterProjectList);
    }

    async filterProjectList(e) {
        const userId = this.userContainer.getAttribute("data-id");
        const searchTerm = e.target.value.toLowerCase();

        try {
            const res = await fetch(`/user/filterProjectsByNameReturnPartial?id=${userId}&searchTerm=${searchTerm}`);
            const projectListHTML = await res.text();
            document.getElementById("projectListContainer").innerHTML = projectListHTML;
        } catch (err) {
            console.error(err);
        }
    }

    async filterTicketList(e) {
        const userId = this.userContainer.getAttribute("data-id");
        const searchTerm = e.target.value.toLowerCase();

        try {
            const res = await fetch(`/user/filterTicketsReturnPartial?id=${userId}&searchTerm=${searchTerm}`);
            const ticketListHTML = await res.text();
            document.getElementById("ticketListContainer").innerHTML = ticketListHTML;
        } catch (err) {
            console.error(err);
        }
    }
}

const user = new User();
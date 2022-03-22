
class Dashboard {
    constructor() {
        console.log("Initialized Dashboard");
        this.filterUserRolesTicketsHistoryList = this.filterUserRolesTicketsHistoryList.bind(this);
        this.events();
    }

    events() {
        document.getElementById("historyListSearchInput").addEventListener("keyup", this.filterUserRolesTicketsHistoryList);
    }

    async filterUserRolesTicketsHistoryList(e) {
        const searchTerm = e.target.value.toLowerCase();

        try {
            const res = await fetch(`/ticketHistory/filterUserRoleTicketsHistoryReturnPartial?searchTerm=${searchTerm}`);
            const historyListHTML = await res.text();
            document.getElementById("historyListContainer").innerHTML = historyListHTML;
        } catch (err) {
            console.error(err);
        }
    }
}

const dashboard = new Dashboard();
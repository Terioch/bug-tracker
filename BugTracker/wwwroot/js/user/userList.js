
class UserList {
    constructor() {
        console.log("Initialized UserList")
        this.filterUserList = this.filterUserList.bind(this);
        this.events();
    }

    events() {
        document.getElementById("userListSearchInput").addEventListener("keyup", this.filterUserList);
    }

    async filterUserList(e) {        
        const searchTerm = e.target.value.toLowerCase();

        try {
            const res = await fetch(`/user/filterUsersByNameReturnPartial?searchTerm=${searchTerm}`);
            const ticketListHTML = await res.text();
            document.getElementById("userListContainer").innerHTML = ticketListHTML;
        } catch (err) {
            console.error(err);
        }
    }
}

const userList = new UserList();
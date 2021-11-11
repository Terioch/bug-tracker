
class Roles {
    constructor() {
        if (document.getElementById("rolesMain")) {
            console.log("Initialized Roles");
            this.rolesMain = document.getElementById("rolesMain");
            this.clickHandler = this.clickHandler.bind(this);          
            this.events();
        }       
    }

    events() {
        this.rolesMain.addEventListener("click", this.clickHandler);
    }

    clickHandler(e) {
        if (e.target.classList.contains("edit-role")) this.handleEdit(e);
        else if (e.target.classList.contains("update-role")) this.handleUpdate(e);
    }

    findNearestParentCard(el) {
        while (!el.classList.contains("role-card")) {
            el = el.parentElement;
        }
        return el;
    }

    makeRoleFieldEditable(el) {
        el.removeAttribute("disabled");
        el.classList.remove("border-0");
        el.classList.add("border-secondary");
    }

    makeRoleFieldReadonly(el) {
        el.setAttribute("disabled");
        el.classList.remove("border-secondary");
        el.classList.add("border-0");
    }

    handleEdit(e) {
        const editBtn = e.target;
        const thisCard = this.findNearestParentCard(e.target);  
        this.makeRoleFieldEditable(thisCard.querySelector(".role-name-input"));
        editBtn.classList.remove("edit-role");
        editBtn.classList.add("update-role");
        editBtn.textContent = "Save";
    }

    async handleUpdate(e) {
        console.log("update", e.target);
        const res = await fetch("/ad")
    }

    async handleDelete(e) {
        console.log(e.target);
    }
}

export default Roles;
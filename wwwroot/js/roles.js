
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
        if (e.target.classList.contains("update-role")) this.handleUpdate(e);
    }

    findNearestParentCard(el) {
        while (!el.classList.contains("role-card")) {
            el = el.parentElement;
        }
        return el;
    }

    handleEdit(e) {
        console.log("editBtn", e.target);
        const editBtn = e.target;
        const thisCard = this.findNearestParentCard(e.target);
        console.log("thisCard", thisCard);
        const roleNameField = thisCard.querySelector(".role-name-input");
        console.log("disabled roleNameField", roleNameField);
        roleNameField.removeAttribute("disabled");
        console.log("editable roleNameField", roleNameField);
        editBtn.classList.remove("edit-role");
        editBtn.classList.add("update-role");
        editBtn.textContent = "Save";
    }

    async handleUpdate(e) {
        console.log(e.target);
    }

    async handleDelete(e) {
        console.log(e.target);
    }
}

export default Roles;
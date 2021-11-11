
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
        el.setAttribute("disabled", "");
        el.classList.remove("border-secondary");
        el.classList.add("border-0");
    }

    getRoleUsers(thisRole) {
        const userCollection = thisRole.querySelector(".role-user-list").children;
        const users = [...[...userCollection].map(u => u.textContent)];
        return users;
    }

    getRoleModel(thisRole) {
        return {
            id: thisRole.getAttribute("data-id"),
            name: thisRole.querySelector(".role-name-input").value,
            users: this.getRoleUsers(thisRole)
        };
    }

    handleEdit(e) {
        const editBtn = e.target;
        const thisRole = this.findNearestParentCard(e.target);  
        this.makeRoleFieldEditable(thisRole.querySelector(".role-name-input"));
        editBtn.classList.remove("edit-role");
        editBtn.classList.add("update-role");
        editBtn.textContent = "Save";
    }

    handleUpdateCleanup(editBtn, thisRole) {
        this.makeRoleFieldReadonly(thisRole.querySelector(".role-name-input"));
        editBtn.classList.remove("update-role");
        editBtn.classList.add("edit-role");
        editBtn.textContent = "Edit";
    }

    async handleUpdate(e) {
        const thisRole = this.findNearestParentCard(e.target); 
        const model = this.getRoleModel(thisRole);        
        const res = await fetch(`/role/update`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(model)
        });
        const data = await res.json();
     
        if (data[0]?.code) {
            for (let error of data) {
                const errorList = thisRole.querySelector(".error-list");
                const errorListItem = `<li class="list-group-item border-0 text-danger">${error.description}</li>`;
                errorList.insertAdjacentHTML("beforeend", errorListItem);
            }
        }                   
        this.handleUpdateCleanup(e.target, thisRole);
    }

    async handleDelete(e) {
        console.log(e.target);
    }
}

export default Roles;

class Roles {
    constructor() {
        if (document.getElementById("rolesMain")) {
            console.log("Initialized Roles");
            this.rolesMain = document.getElementById("rolesMain");
            this.clickHandler = this.clickHandler.bind(this);           
            this.model = {
                id: -1,
                name: "",
                users: []
            };
            this.populateUserDropdown();
            this.events();
        }       
    }

    events() {
        this.rolesMain.addEventListener("click", this.clickHandler);
    }

    clickHandler(e) {
        if (e.target.classList.contains("edit-role")) this.handleEdit(e);
        else if (e.target.classList.contains("update-role")) this.handleUpdate(e);
        else if (e.target.classList.contains("delete-role")) this.handleDelete(e);
        else if (e.target.classList.contains("display-user-dropdown-modal")) this.onUserDropdownModalOpen(e);
        else if (e.target.classList.contains("add-user")) this.handleUserAddition(e);
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

    setSelectedUser(e) {
        selectedUser = e.target.value;
    }

    async populateUserDropdown() {
        const userDropdown = document.getElementById("userDropdown");
        console.log(userDropdown);

        try {
            const res = await fetch('/role/listUsers');
            const data = await res.json();
            console.log(data);
            for (let item of data) {
                const dropdownItem = `<a class="dropdown-item user-dropdown-item">${item.userName}</a>`;
                userDropdown.insertAdjacentHTML("beforeend", dropdownItem)
            }
        } catch (err) {
            console.error(err);
        }
    }

    onUserDropdownModalOpen(e) {
        const thisRole = this.findNearestParentCard(e.target);
        const userDropdown = document.getElementById("userDropdown");
        const addUserBtn = document.querySelector(".add-user");
        const selectedUser = "";
        userDropdown.addEventListener("change", () => this.setSelectedUser(selectedUser));
        addUserBtn.addEventListener("click", () => this.handleUserAddition(thisRole, selectedUser));
    }

    handleEdit(e) {
        const editBtn = e.target;
        const thisRole = this.findNearestParentCard(e.target);
        this.model = this.getRoleModel(thisRole);
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
        const errorList = thisRole.querySelector(".error-list");
        errorList.innerHTML = "";

        try {
            const res = await fetch(`/role/update`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(model)
            });
            const data = await res.json();            

            if (data.errors) {
                for (let error of data.errors) {                    
                    const errorListItem = `<li class="list-group-item border-0 text-danger">${error.description}</li>`;
                    errorList.insertAdjacentHTML("beforeend", errorListItem);
                }             
                thisRole.querySelector(".role-name-input").value = this.model.name;
            }
            this.handleUpdateCleanup(e.target, thisRole);
        } catch (err) {
            console.error(err);
        }        
    }

    async handleDelete(e) {
        const thisRole = this.findNearestParentCard(e.target);
        const id = thisRole.getAttribute("data-id");
        const errorList = thisRole.querySelector(".error-list");
        errorList.innerHTML = "";

        try {
            const res = await fetch(`/role/delete/${id}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json"
                }             
            });
            const data = await res.text();
            thisRole.parentElement.remove(data);
        } catch (err) {
            console.error(err);
        }
    }

    async handleUserAddition(thisRole, selectedUser) {       
        const roleId = thisRole.getAttribute("data-id");
        console.log(thisRole);
        console.log(roleId);

        try {
            const res = await fetch(`/role/addUser/${roleId}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: selectedUser
            });
            const data = await res.json();
            console.log(data);
        } catch (err) {
            console.error(err);
        }
    }
}

export default Roles;
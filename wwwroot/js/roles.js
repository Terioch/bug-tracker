
class Roles {
    constructor() {
        if (document.getElementById("rolesMain")) {
            console.log("Initialized Roles");            
            this.selectedUser = "";
            this.model = {
                id: -1,
                name: "",
                users: []
            };
            this.clickHandler = this.clickHandler.bind(this);
            this.onUserDropdownModalOpen = this.onUserDropdownModalOpen.bind(this);
            this.populateUserDropdown();
            this.events();
        }       
    }

    events() {
        document.getElementById("rolesMain").addEventListener("click", this.clickHandler);
        document.querySelector(".display-user-dropdown-modal").addEventListener("click", this.onUserDropdownModalOpen);
        document.getElementById("userSelectDropdown").addEventListener("change", (e) => this.setSelectedUser(e));
    }

    clickHandler(e) {
        if (e.target.classList.contains("edit-role")) this.handleEdit(e);
        else if (e.target.classList.contains("update-role")) this.handleUpdate(e);
        else if (e.target.classList.contains("delete-role")) this.handleDelete(e);      
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
        this.selectedUser = e.target.value;
    }

    onUserDropdownModalOpen(e) {
        const thisRole = this.findNearestParentCard(e.target);
        const addUserBtn = document.querySelector(".add-user");
        addUserBtn.addEventListener("click", (e) => this.handleUserAddition(e, thisRole));
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

    async populateUserDropdown() {
        const userDropdown = document.getElementById("userSelectDropdown");

        try {
            const res = await fetch('/role/listUsers');
            const data = await res.json();
            this.selectedUser = data[0].userName;
            for (let item of data) {
                const dropdownItem = `<option>${item.userName}</option>`;
                userDropdown.insertAdjacentHTML("beforeend", dropdownItem);
            }
        } catch (err) {
            console.error(err);
        }
    }

    async handleUpdate(e) {       
        const thisRole = this.findNearestParentCard(e.target);
        const model = this.getRoleModel(thisRole);
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

    async handleUserAddition(e, thisRole) {
        e.stopPropagation(); // Prevent more than one method execution
        const roleId = thisRole.getAttribute("data-id"); 

        try {
            const res = await fetch(`/role/addUser/${roleId}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ userName: this.selectedUser })
            });
            const data = await res.json();
            console.log(data);
        } catch (err) {
            console.error(err);
        }
    }
}

export default Roles;
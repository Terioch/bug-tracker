
class Roles {
    constructor() {
        if (document.getElementById("rolesMain")) {
            console.log("Initialized Roles");            
            this.selectedUser = {
                id: "",
                userName: ""
            };
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
        document.getElementById("userSelectDropdown").addEventListener("change", (e) => this.setSelectedUser(e));
        document.getElementById("rolePagination").addEventListener("click", (e) => {
            if (e.target.classList.contains("page-link")) {
                this.getCurrentRole(e);
            }
        });
    }

    clickHandler(e) {
        if (e.target.classList.contains("edit-role")) this.handleEdit(e);
        else if (e.target.classList.contains("update-role")) this.handleUpdate(e);
        else if (e.target.classList.contains("delete-role")) this.handleDelete(e);
        else if (e.target.classList.contains("user-dropdown-modal-icon")) this.onUserDropdownModalOpen(e);
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
        [...e.target.children].forEach(item => {
            if (item.value == e.target.value) {
                this.selectedUser = {
                    id: item.id,
                    userName: item.getAttribute("data-userName")
                };
            }
        });
    }

    onUserDropdownModalOpen(e) {
        const thisRole = this.findNearestParentCard(e.target);        
        const addUserBtn = document.querySelector(".add-user");
        const removeUserBtn = document.querySelector(".remove-user");
        addUserBtn.addEventListener("click", (e) => this.handleUserAddition(e, thisRole));
        removeUserBtn.addEventListener("click", (e) => this.handleUserRemoval(e, thisRole));
    }

    async getCurrentRole(e) {
        

        switch (e.target.value) {
            case "Previous":
                console.log(e.target.value);
                break;
            case "1":
                console.log(e.target.value);
                break;
            case "2":
                console.log(e.target.value);
                break;
            case "Next":
                console.log(e.target.value);
                break;
        }
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
            this.selectedUser = {
                id: data[0].id,
                userName: data[0].userName
            };

            for (let item of data) {
                const dropdownItem = `<option id="${item.id}" data-userName="${item.userName}">${item.firstName} ${item.lastName}</option>`;
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
            const res = await fetch(`/role/addUser`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    roleId,
                    userId: this.selectedUser.id,
                    userName: this.selectedUser.userName
                })
            });
            const data = await res.json();
            console.log(data);
        } catch (err) {
            console.error(err);
        }
    }

    async handleUserRemoval(e, thisRole) {
        e.stopPropagation(); // Prevent more than one method execution
        const roleId = thisRole.getAttribute("data-id");

        try {
            const res = await fetch(`/role/removeUser`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    roleId,
                    userId: this.selectedUser.id,
                    userName: this.selectedUser.userName
                })
            });
            const data = await res.json();
            console.log(data);
        } catch (err) {
            console.error(err);
        }
    }
}

export default Roles;
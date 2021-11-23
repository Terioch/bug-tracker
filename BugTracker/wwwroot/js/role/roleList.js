
class RoleList {
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
            this.paginateRoles = this.paginateRoles.bind(this);
            this.populateUserDropdown();
            this.events();
        }       
    }

    events() {
        document.getElementById("rolesMain").addEventListener("click", this.clickHandler);
        document.getElementById("userSelectDropdown").addEventListener("change", (e) => this.setSelectedUser(e));
        // document.getElementById("rolePagination").addEventListener("click", this.paginateRoles);
    }

    clickHandler(e) {
        if (e.target.classList.contains("edit-role")) this.handleEdit(e);
        else if (e.target.classList.contains("update-role")) this.handleUpdate(e);
        else if (e.target.classList.contains("delete-role")) this.handleDelete(e);
        else if (e.target.classList.contains("edit-user-list-onload")) this.onUserDropdownModalOpen(e);
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

    getRoleModel(thisRole) {
        return {
            id: thisRole.getAttribute("data-id"),
            name: thisRole.querySelector(".role-name-input").value,
            users: this.getRoleUsers(thisRole)
        };
    }

    getRoleUsers(thisRole) {
        const userCollection = thisRole.querySelector(".role-user-list").children;
        let users = [];
        [...[...userCollection].map(ul => (
            [...ul.children].map(li => (
                users.push(li.textContent)
            ))
        ))];
        return users;
    } 

    getNewRoleIndex(index, predicate) {
        if (predicate === "Previous") index--;          
        else if (predicate === "1") index = 0;
        else if (predicate === "2") index = 1;
        else if (predicate === "3") index = 3;
        else index++;
        return index;
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
        e.target.classList.remove("edit-user-list-onload"); // Prevent more than one method execution
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

    async paginateRoles(e) {
        if (!e.target.classList.contains("page-link")) return;

        const thisRole = this.findNearestParentCard(e.target);
        const thisRoleParent = thisRole.parentElement;
        let roleIndex = thisRole.getAttribute("data-index");
        roleIndex = this.getNewRoleIndex(roleIndex, e.target.textContent);

        try {
            const res = await fetch(`/role/getCurrentRoleReturnPartial/${roleIndex}`);
            const roleCardHTML = await res.text();
            thisRoleParent.innerHTML = roleCardHTML;
            document.getElementById("rolePagination").addEventListener("click", this.paginateRoles); // Add listener to the new role card pagination element
        } catch (err) {
            console.error(err);
        }
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
        const thisRoleUserList = thisRole.querySelector(".role-user-list-container");
        console.log("add user");
       
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

            if (res.status === 500) throw await res.text();        
            const userListHTML = await res.text();   
            thisRoleUserList.innerHTML = userListHTML;
        } catch (err) {
            console.error(err);
        }
    }

    async handleUserRemoval(e, thisRole) {
        e.stopPropagation(); // Prevent more than one method execution
        const roleId = thisRole.getAttribute("data-id");
        const thisRoleUserList = thisRole.querySelector(".role-user-list-container");
        console.log("remove user");

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
            
            if (res.status === 500) throw await res.text();
            const userListHTML = await res.text();
            thisRoleUserList.innerHTML = userListHTML;       
        } catch (err) {
            console.error(err);
        }
    }
}

export default RoleList;
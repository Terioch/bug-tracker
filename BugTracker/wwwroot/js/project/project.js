
class Project {
    constructor() {
        if (document.getElementById("projectContainer")) {
            console.log("Initialized Project");
            this.projectContainer = document.getElementById("projectContainer");
            this.handleDelete = this.handleDelete.bind(this);
            this.onUsersManagementModalOpen = this.onUsersManagementModalOpen.bind(this);
            this.events();
        }          
    }

    events() {
        document.getElementById("deleteProjectBtn").addEventListener("click", this.handleDelete);
        document.getElementById("manageUsersBtn").addEventListener("click", this.onUsersManagementModalOpen);
    }

    async handleDelete() {
        const id = this.projectContainer.getAttribute("data-id");       

        try {
            const res = await fetch(`/project/delete/${id}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "Application/Json",
                }
            });

            if (res.url) {
                window.location.href = res.url;
                return;
            }              
            await res.json();         
            window.location.href = "/Project/ListProjects";
        } catch (err) {
            console.error(err);
        }
    }    

    onUsersManagementModalOpen() {       
        const addUserBtn = document.querySelector(".add-user");
        const removeUserBtn = document.querySelector(".remove-user");
        addUserBtn.addEventListener("click", this.handleUserAddition.bind(this));
        removeUserBtn.addEventListener("click", this.handleUserRemoval.bind(this));    
    }

    async handleUserAddition() {        
        const userName = document.getElementById("userNameInput").value;
        const id = this.projectContainer.getAttribute("data-id");

        try {
            const res = await fetch(`/project/addUser/${id}?userName=${userName}`, {
                method: "POST",
                headers: {
                    "Content-Type": "Application/Json",
                }
            });

            if (res.url) {
                window.location.href = res.url;
                return;
            }              
            if (res.status === 400) throw await res.json();            

            const data = await res.json();
            window.location.reload();
        } catch (err) {
            console.error(err);
            const validationErrorsContainer = document.getElementById("projectUsersManagementvalidationErrors");
            validationErrorsContainer.innerHTML = err.message;
        }
    }

    async handleUserRemoval() {
        const userName = document.getElementById("userNameInput").value;     
        const id = this.projectContainer.getAttribute("data-id");        

        try {
            const res = await fetch(`/project/removeUser/${id}?userName=${userName}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "Application/Json",
                }
            });

            if (res.url) {
                window.location.href = res.url;
                return;
            }              
            if (res.status === 400) throw await res.json();              

            const data = await res.json();
            window.location.reload();
        } catch (err) {
            console.error(err);
            const validationErrorsContainer = document.getElementById("projectUsersManagementvalidationErrors");
            validationErrorsContainer.innerHTML = err.message;
        }
    }
}

export default Project;
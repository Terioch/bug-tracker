
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

            if (res.status === 400 && res.url) {
                window.location.href = res.url;
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
        addUserBtn.addEventListener("click", this.handleUserAddition);
        removeUserBtn.addEventListener("click", this.handleUserRemoval);    
    }

    async handleUserAddition() {        
        const userNameInput = document.getElementById("userNameInput");
        console.log(userNameInput.value);
    }

    async handleUserRemoval() {
        const userNameInput = document.getElementById("userNameInput");
        console.log(userNameInput.value);
    }
}

export default Project;
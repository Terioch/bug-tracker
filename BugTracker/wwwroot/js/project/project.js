
class Project {
    constructor() {
        if (document.getElementById("projectContainer")) {
            console.log("Initialized Project");
            this.projectContainer = document.getElementById("projectContainer");
            this.handleDelete = this.handleDelete.bind(this);
            this.events();
        }          
    }

    events() {
        document.getElementById("deleteProjectBtn").addEventListener("click", this.handleDelete);
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
}

export default Project;
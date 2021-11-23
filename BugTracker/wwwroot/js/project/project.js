
class Project {
    constructor() {
        if (document.getElementById("projectInfoContainer")) {
            console.log("Project Initialized");
            this.handleDelete = this.handleDelete.bind(this);
            this.events();
        }               
    }

    events() {
        document.getElementById("deleteProjectBtn").addEventListener("click", this.handleDelete);
    }

    handleDelete() {
        console.log("delete project");
    }
}

export default Project;
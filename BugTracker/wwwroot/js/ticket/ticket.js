
class Ticket {
    constructor() {
        if (document.getElementById("ticketContainer")) {
            console.log("Initialized Ticket");
            this.ticketContainer = document.getElementById("ticketContainer");
            this.handleDelete = this.handleDelete.bind(this);
            this.handleCommentCreation = this.handleCommentCreation.bind(this);
            this.commentListClickHandler = this.commentListClickHandler.bind(this);
            this.events();
        }
    }

    events() {
        document.getElementById("deleteTicketBtn").addEventListener("click", this.handleDelete);
        document.getElementById("createCommentBtn").addEventListener("click", this.handleCommentCreation);
        document.getElementById("commentListGroup").addEventListener("click", this.commentListClickHandler);
    }

    findNearestParentElement(el, className) {
        while (!el.classList.contains(className)) {
            el = el.parentElement;
        }
        return el;
    }

    async handleDelete() {
        const id = this.ticketContainer.getAttribute("data-id");
        console.log("delete");
        try {
            const res = await fetch(`/ticket/delete/${id}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "Application/Json",
                }
            });

            if (res.status === 400 && res.url) {
                window.location.href = res.url;
            }
            await res.json();    
            window.location.href = "/Ticket/ListTickets";
        } catch (err) {
            console.error(err);
        }
    }

    commentListClickHandler(e) {
        const commentId = this.findNearestParentElement(e.target, "comment-list-item").getAttribute("data-commentId");
        console.log(commentId);

        if (e.target.classList.contains("delete-comment-trigger")) {
            const deleteCommentBtn = document.getElementById("deleteCommentBtn");         

            if (!deleteCommentBtn.getAttribute("data-listening")) {
                deleteCommentBtn.addEventListener("click", () => this.handleCommentDeletion(commentId));
                deleteCommentBtn.setAttribute("data-listening", true);
                console.log("listening");
            } else {
                deleteCommentBtn.removeEventListener("click", () => this.handleCommentDeletion(commentId));
                deleteCommentBtn.removeAttribute("data-listening");
            }
        }
    }

    async handleCommentCreation() {
        const id = this.ticketContainer.getAttribute("data-id");
        const commentsContainer = document.getElementById("commentListContainer"); 
        const commentDescriptionInput = document.getElementById("commentDescriptionInput");

        try {
            const res = await fetch(`/ticket/createComment/${id}`, {
                method: "POST",
                headers: {
                    "Content-Type": "Application/Json",
                },
                body: JSON.stringify({
                    value: commentDescriptionInput.value,
                })
            });
            
            const commentListHTML = await res.text();                        
            commentsContainer.innerHTML = commentListHTML;
            commentDescriptionInput.value = "";
        } catch (err) {
            console.error(err);
        }
    }

    async handleCommentDeletion(commentId) {
        const commentsContainer = document.getElementById("commentListContainer");         
        console.log(commentId);

        try {
            const res = await fetch(`/ticket/deleteComment/${commentId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "Application/Json",
                },
            });

            const commentListHTML = await res.text();            
            commentsContainer.innerHTML = commentListHTML;

            // Remove listener and listening attribute for this comment
            const deleteCommentBtn = document.getElementById("deleteCommentBtn");   
            deleteCommentBtn.removeEventListener("click", () => this.handleCommentDeletion(commentId));
            deleteCommentBtn.removeAttribute("data-listening");
        } catch (err) {
            console.error(err);
        }
    }
}

export default Ticket;
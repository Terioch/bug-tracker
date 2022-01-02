﻿
class Ticket {
    constructor() {
        if (document.getElementById("ticketContainer")) {
            console.log("Initialized Ticket");
            this.ticketContainer = document.getElementById("ticketContainer");
            this.handleDelete = this.handleDelete.bind(this);
            this.handleCommentCreation = this.handleCommentCreation.bind(this);
            this.events();
        }
    }

    events() {
        document.getElementById("deleteTicketBtn").addEventListener("click", this.handleDelete);
        document.getElementById("createCommentBtn").addEventListener("click", this.handleCommentCreation);
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
}

export default Ticket;
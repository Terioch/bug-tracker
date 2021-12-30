
class Ticket {
    constructor() {
        if (document.getElementById("ticketContainer")) {
            console.log("Initialized Ticket");
            this.ticketContainer = document.getElementById("ticketContainer");
            this.handleDelete = this.handleDelete.bind(this);
            this.events();
        }
    }

    events() {
        document.getElementById("deleteTicketBtn").addEventListener("click", this.handleDelete);
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
        const comment = {
            value: this.ticketContainer.GetElementById("commentDescriptionInput").value,
        };

        try {
            const res = await fetch(`/ticket/addComment/${id}`, {
                method: "POST",
                headers: {
                    "Content-Type": "Application/Json",
                },
                body: { comment }
            });

            /*if (res.status === 400 && res.url) {
                window.location.href = res.url;
            }*/
            const data = await res.json();
            console.log(data);
        } catch (err) {
            console.error(err);
        }
    }
}

export default Ticket;
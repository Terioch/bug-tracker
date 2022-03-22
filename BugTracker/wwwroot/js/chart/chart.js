/*Colours:
Blue: #417FEF
Grey: #ADB5BD
Light Blue: #17A2B8
Dark Grey: #343A40
Green: #00B973
Orange: #F0AD4E
Red: #D9534F*/

class ChartRenderer {
    constructor() {
        console.log("Initialized Chart");
        // Add additional spacing between chart and legend labels
        /*Chart.Legend.prototype.afterFit = function () {
            this.height = this.height + 50;
        };*/       
        this.renderPriorityChart();
        this.renderStatusChart();
        this.renderTypeChart();
    }

    // Render view and then fetch data from the server
    async renderTypeChart() {
        const context = document.getElementById("ticketTypeChart");
        const response = await fetch("chart/getTicketTypeData");
        const data = await response.json();

        const ticketTypeChart = new Chart(context, {
            type: "doughnut",
            data: {
                labels: data.labels,
                datasets: [{
                    label: "Tickets by Type",
                    backgroundColor: [
                        '#D9534F',
                        '#417FEF',
                        '#00B973',
                        '#F0AD4E',
                    ],
                    data: data.values,
                }],
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: "bottom",       
                        textAlign: "left"
                    }
                },
                animation: {
                    animateScale: true,
                    animateRotate: true,
                    animationSteps: 100,
                    animationEasing: 'easeOutBounce',
                }
            }
        });
    }

    async renderStatusChart() {
        const context = document.getElementById("ticketStatusChart");
        const response = await fetch("chart/getTicketStatusData");
        const data = await response.json();

        const ticketTypeChart = new Chart(context, {
            type: "bar",
            data: {
                labels: data.labels,
                datasets: [{
                    label: "Tickets by Status",
                    backgroundColor: [
                        '#ADB5BD',
                        '#F0AD4E',
                        '#417FEF',
                        '#17A2B8',
                        '#00B973',
                    ],
                    data: data.values,
                }],
            },
            options: {
                responsive: true,
                indexAxis: "x",
                plugins: {
                    legend: {
                        display: false
                    }
                },
                animation: {
                    animateScale: true,
                    animateRotate: true,
                    animationSteps: 100,
                    animationEasing: 'easeOutBounce',
                }
            }
        });
    }

    async renderPriorityChart() {
        const context = document.getElementById("ticketPriorityChart");
        const response = await fetch("chart/getTicketPriorityData");
        const data = await response.json();

        const ticketPriorityChart = new Chart(context, {
            type: "pie",
            data: {
                labels: data.labels,
                datasets: [{
                    label: "Tickets by Priority",
                    backgroundColor: [
                        '#00B973',
                        '#417FEF',
                        '#D9534F',
                    ],
                    data: data.values,
                }],
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: "bottom",
                        textAlign: "left"
                    }
                },
                animation: {
                    animateScale: true,
                    animateRotate: true,
                    animationSteps: 100,
                    animationEasing: 'easeOutBounce',
                }
            }
        });
    }
}

const chartRenderer = new ChartRenderer();
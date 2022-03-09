
class Chart {
    constructor() {
        console.log("Initialized Chart");
        console.log(google);
        google.charts.load('current', { packages: ['corechart'] });
        google.charts.setOnLoadCallback(this.drawChart);
    }

    drawChart() {
        // Define the chart to be drawn.
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Element');
        data.addColumn('number', 'Percentage');
        data.addRows([
            ['Nitrogen', 0.31],
            ['Oxygen', 0.19],
            ['Hydrogen', 0.54],
            ['Other', 0.06]
        ]);

        // Instantiate and draw the chart.
        var chart = new google.visualization.PieChart(document.getElementById('myPieChart'));
        chart.draw(data, null);
    }
}

const chart = new Chart();
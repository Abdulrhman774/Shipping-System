(function($) {
  'use strict';
  $(function() {
      // Get context with jQuery - using jQuery's .get() method.
  if ($("#barChart").length) {
    var data = {
      labels: ["2011", "2012", "2013", "2014", "2015", "2016"],
      datasets: [{
        label: '# of Votes',
        data: [25, 45, 10, 35, 15, 10], 
        backgroundColor: [
          'rgba(255, 99, 132, 0.2)',
          'rgba(54, 162, 235, 0.2)',
          'rgba(255, 206, 86, 0.2)',
          'rgba(75, 192, 192, 0.2)',
          'rgba(153, 102, 255, 0.2)',
          'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
          'rgba(255,99,132,1)',
          'rgba(54, 162, 235, 1)',
          'rgba(255, 206, 86, 1)',
          'rgba(75, 192, 192, 1)',
          'rgba(153, 102, 255, 1)',
          'rgba(255, 159, 64, 1)'
        ],
        borderWidth: 1,
        fill: false
      }]
    };
    var options = {
      scales: {
        yAxes: [{
          ticks: {
            beginAtZero: true,
            stepSize : 10,
          },
          gridLines: {
            display: true,
            drawBorder: false,
            color: "#f5f1f5"
          },
        }],
        xAxes: [{
          ticks: {
          },
          gridLines: {
            display: false,
            drawBorder: false,
          },
        }]
      },
      legend: {
        display: false
      },
      elements: {
        point: {
          radius: 0
        }
      }
  
    };
    var barChartCanvas = $("#barChart").get(0).getContext("2d");
    // This will get the first returned node in the jQuery collection.
    var barChart = new Chart(barChartCanvas, {
      type: 'bar',
      data: data,
      options: options
    });
  }
  });
})(jQuery);

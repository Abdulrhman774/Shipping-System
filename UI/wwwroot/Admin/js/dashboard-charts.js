document.addEventListener("DOMContentLoaded", function () {
    
    // 1. Doughnut Chart
    const ctxDoughnut = document.getElementById('shipmentsByStatusChart');
    if (ctxDoughnut) {
        new Chart(ctxDoughnut, {
            type: 'doughnut',
            data: {
                labels: ['Created', 'Approved', 'Ready to Ship', 'Shipped', 'Delivered', 'Returned'],
                datasets: [{
                    data: [186, 145, 98, 312, 420, 25],
                    backgroundColor: ['#5e5ce6', '#3b82f6', '#6ee7b7', '#10b981', '#064e3b', '#ef4444'],
                    borderWidth: 0
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '65%',
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    }

    // 2. Bar Chart
    const ctxBar = document.getElementById('monthlyVolumeChart');
    if (ctxBar) {
        new Chart(ctxBar, {
            type: 'bar',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                datasets: [{
                    label: 'Volume',
                    data: [320, 380, 350, 420, 480, 560],
                    backgroundColor: '#5e5ce6',
                    borderRadius: 6,
                    barPercentage: 0.6
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false } },
                scales: {
                    y: { beginAtZero: true, grid: { borderDash: [2, 4] } },
                    x: { grid: { display: false } }
                }
            }
        });
    }

    // 3. Line Chart
    const ctxLine = document.getElementById('revenueCostChart');
    if (ctxLine) {
        new Chart(ctxLine, {
            type: 'line',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                datasets: [
                    {
                        label: 'Gross Revenue',
                        data: [11200, 13500, 15800, 14200, 16900, 21000],
                        borderColor: '#5e5ce6',
                        tension: 0.4,
                        fill: false
                    },
                    {
                        label: 'Operational Cost',
                        data: [8200, 9100, 10500, 9800, 8900, 9500],
                        borderColor: '#ef4444',
                        tension: 0.4,
                        fill: false
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { position: 'top' } },
                scales: {
                    y: { beginAtZero: true, grid: { borderDash: [2, 4] } },
                    x: { grid: { display: false } }
                }
            }
        });
    }

    // 4. Horizontal Bar Chart
    const ctxHBar = document.getElementById('topShippersChart');
    if (ctxHBar) {
        new Chart(ctxHBar, {
            type: 'bar',
            data: {
                labels: ['Company A', 'Company B', 'Company C', 'Company D', 'Company E'],
                datasets: [{
                    label: 'Shipments',
                    data: [142, 128, 115, 98, 82],
                    backgroundColor: '#5e5ce6',
                    borderRadius: 4
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false } },
                scales: {
                    x: { beginAtZero: true, grid: { borderDash: [2, 4] } },
                    y: { grid: { display: false } }
                }
            }
        });
    }

});

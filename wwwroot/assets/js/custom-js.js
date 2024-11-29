function newTable(params) {
    var table = $(params).DataTable({
        
    });
};
// دریافت داده‌های بدهی‌ها برای چارت
async function loadDebtChartData(api,chartId,chartType) {


    const response = await fetch(api);
    const data = await response.json();
    // نمایش داده‌ها در کنسول

    const labels = data.map(item => item.userName);

    const remainingDebts = data.map(item => item.remainingDebt);

    const ctx = document.getElementById(chartId).getContext('2d');
    new Chart(ctx, {
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                data: remainingDebts,
                backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', '#f6c23e', '#e74a3b'],
                hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', '#f4b619', '#e02d1b'],
                borderWidth: 1
            }]
        }
    });
}
async function loadValueChartData(api,tableId,chartLabel) {
    
fetch(api)
.then(response => response.json())
.then(data => {
    if (data.length === 0) {
        return;
    }
    const labels = data.map(item => item.paymentDate); // برای تاریخ پرداخت
    const values = data.map(item => item.amount); // برای مبلغ       

    const ctx = document.getElementById(tableId).getContext('2d');
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: chartLabel,
                data: values,
                borderColor: [
                    'rgb(255, 99, 132)',
                    'rgb(255, 159, 64)',
                    'rgb(255, 205, 86)',
                    'rgb(75, 192, 192)',
                    'rgb(54, 162, 235)',
                    'rgb(153, 102, 255)',
                    'rgb(201, 203, 207)'
                ], backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(255, 159, 64, 0.2)',
                    'rgba(255, 205, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(201, 203, 207, 0.2)'
                ], borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
})
.catch(error => {
    console.error('خطا در دریافت داده‌ها:', error);
    alert('مشکلی در بارگذاری داده‌ها رخ داده است.');
});




}
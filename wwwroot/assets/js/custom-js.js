function newTable(params) {
    var table = $(params).DataTable({
    
        language: {
            url: 'https://cdn.datatables.net/plug-ins/2.1.8/i18n/fa.json',
        },

    });
};
// دریافت داده‌های بدهی‌ها برای چارت
async function loadAllDebtChartData(api, chartId, chartType) {


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
async function loadPaymentsChartData(api, tableId, chartLabel) {

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

async function loaddebtsChartData(api, tableId) {
    fetch(api)
    .then(response => response.json())
    .then(data => {
        const chartData = data.chartData; // دسترسی به آرایه chartData

        const labels = chartData.map(item => item.title);
        const debtAmounts = chartData.map(item => item.debtAmount);
        const paymentAmounts = chartData.map(item => item.paymentAmount);


        // رسم نمودار با Chart.js
        const ctx = document.getElementById(tableId).getContext('2d');
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'مقدار بدهی',
                        data: debtAmounts,
                        backgroundColor: 'rgba(255, 99, 132, 0.6)',
                        borderColor: 'rgba(54, 162, 235, 1)',  // رنگ مرز میله‌ها
                        borderWidth: 1,  // عرض مرز
                        hoverBackgroundColor: 'rgba(54, 162, 235, 0.8)',  // رنگ میله‌ها هنگام هاور
                        hoverBorderColor: 'rgba(54, 162, 235, 1)',  // رنگ مرز هنگام هاور
                        barThickness: 40,  // ضخامت میله‌ها
                        borderRadius: 50,  // گوشه‌های گرد میله‌ها
                    },
                    {
                        label: 'مجموع پرداخت‌ها',
                        data: paymentAmounts,
                        backgroundColor: 'rgba(54, 162, 235, 0.6)',
                        borderColor: 'rgba(54, 162, 235, 1)',  // رنگ مرز میله‌ها
                        borderWidth: 1,  // عرض مرز
                        hoverBackgroundColor: 'rgba(54, 162, 235, 0.8)',  // رنگ میله‌ها هنگام هاور
                        hoverBorderColor: 'rgba(54, 162, 235, 1)',  // رنگ مرز هنگام هاور
                        barThickness: 40,  // ضخامت میله‌ها
                        borderRadius: 5,  // گوشه‌های گرد میله‌ها
                    }
                ]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            color: '#e0e0e0',  // رنگ خطوط شبکه
                            lineWidth: 1  // عرض خطوط شبکه
                        }
                    },
                    x: {
                        grid: {
                            color: '#f0f0f0',  // رنگ خطوط شبکه محور x
                        }
                    }
                },
                plugins: {
                    tooltip: {
                        backgroundColor: '#333',  // رنگ پس‌زمینه تولتیپ
                        titleColor: '#fff',  // رنگ عنوان تولتیپ
                        bodyColor: '#fff',  // رنگ متن تولتیپ
                        footerColor: '#fff',  // رنگ فوتر تولتیپ
                        borderColor: '#fff',  // رنگ مرز تولتیپ
                        borderWidth: 1  // عرض مرز تولتیپ
                    },
                    legend: {
                        labels: {
                            fontColor: '#000',  // رنگ فونت لیبل‌های legend
                            fontSize: 14  // اندازه فونت
                        }
                    }
                },
                animation: {
                    duration: 1000,  // مدت زمان انیمیشن
                    easing: 'easeInOutQuad'  // نوع انیمیشن
                }
            }
        
        });
    })
    .catch(error => {
        console.error("خطا در پردازش داده‌ها:", error);
    });



}
function convertNumbersToPersian() {
    const persianDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    document.body.innerHTML = document.body.innerHTML.replace(/\d/g, function (w) {
        return persianDigits[+w];
    });
}
// تابع تبدیل اعداد فارسی به انگلیسی
function persianToEnglishNumber(str) {
    return str.replace(/,/g, '').replace(/[۰-۹]/g, function(digit){
        return '۰۱۲۳۴۵۶۷۸۹'.indexOf(digit);
    });
}
function persianToEnglishNumber(str) {
    return str.replace(/,/g, '').replace(/[۰-۹]/g, function(digit) {
        return '۰۱۲۳۴۵۶۷۸۹'.indexOf(digit);
    });
}



    



document.addEventListener("DOMContentLoaded", function () {
  // api url
  const api_url =
    "http://203.115.20.74:5000/api/Common/getallfactorywisesale?status=false";
  const api_url1 =
    "http://203.115.20.74:5000/api/Common/topsellingproduct?status=false";
  const api_url2 =
    "http://203.115.20.74:5000/api/Common/dquantityvsdissueingquantity?status=false";

  // Defining async function
  async function getapi(url) {
    // Storing response
    const response = await fetch(url);
    // Storing data in form of JSON
    // console.log(data);
    if (response.status==200) {
      var data = await response.json();
      if (data) {
        show(data);
      }
    }
  }
  // Defining async function
  async function getapi1(url) {
    // Storing response
    const response = await fetch(url);
    // Storing data in form of JSON
    // console.log(data);
    if (response.status==200) {
      var data = await response.json();
      if (data) {
        show1(data);
      }
    }
  }
  // Defining async function
  async function getapi2(url) {
    // Storing response
    const response = await fetch(url);
    // Storing data in form of JSON
    // console.log(data);
    if (response.status==200) {
      var data3 = await response.json();
      if (data3) {
        show3(data3);
      }
    }
  }
  // Calling that async function
  getapi(api_url);
  getapi1(api_url1);
  getapi2(api_url2);

  // Function to define innerHTML for HTML table
  function show1(data) {
    const cty = document.getElementById("topsellingproduct");
    const myChart1 = new Chart(cty, {
      type: "bar",
      data: {
        labels: data.map((row) => row.productName),
        datasets: [
          {
            label: "This Month Top 30 Product Sales",
            data: data.map((row) => row.productSale),
            backgroundColor: [
              "rgba(255, 99, 132, 0.5)",
              "rgba(54, 162, 235, 0.6)",
              "rgba(255, 206, 86, 0.2)",
              "rgba(75, 192, 192, 0.5)",
              "rgba(153, 102, 255, 0.4)",
              "rgba(255, 159, 64, 0.7)",
            ],
            borderColor: [
              "rgba(255, 99, 132, 1)",
              "rgba(54, 162, 235, 1)",
              "rgba(255, 206, 86, 1)",
              "rgba(75, 192, 192, 1)",
              "rgba(153, 102, 255, 1)",
              "rgba(255, 159, 64, 1)",
            ],
            borderWidth: 1,
          },
        ],
      },
      options: {
        indexAxis: "y",
        scales: {
          y: {
            beginAtZero: true,
          },
        },
        animation: true,
        responsive: true,
        plugins: {
          legend: {
            display: true,
          },
          tooltip: {
            enabled: true,
          },
        },
      },
    });
  }
  // Function to define innerHTML for HTML table
  function show(data) {
    const ctx2 = document.getElementById("factorywisesale");
    const myChart = new Chart(ctx2, {
      type: "bar",
      data: {
        labels: data.map((row) => row.branchCode),
        datasets: [
          {
            label: "This Month Factory Wise Sales",
            data: data.map((row) => row.saleAmount),
            backgroundColor: [
              "rgba(255, 99, 132, 0.5)",
              "rgba(54, 162, 235, 0.6)",
              "rgba(255, 206, 86, 0.2)",
              "rgba(75, 192, 192, 0.5)",
              "rgba(153, 102, 255, 0.4)",
              "rgba(255, 159, 64, 0.7)",
            ],
            borderColor: [
              "rgba(255, 99, 132, 1)",
              "rgba(54, 162, 235, 1)",
              "rgba(255, 206, 86, 1)",
              "rgba(75, 192, 192, 1)",
              "rgba(153, 102, 255, 1)",
              "rgba(255, 159, 64, 1)",
            ],
            borderWidth: 1,
          },
        ],
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
          },
        },
        animation: true,
        responsive: true,
        plugins: {
          legend: {
            display: true,
          },
          tooltip: {
            enabled: true,
          },
        },
      },
    });
  }
  // Function to define innerHTML for HTML table
  function show3(data3) {
    const ctx3 = document.getElementById("dquantityerror");
    const myChart2 = new Chart(ctx3, {
      type: "bar",
      data: {
        labels: data3.map((row) => row.barCode),
        datasets: [
          {
            label: "Stock Quantity Minus",
            data: data3.map((row) => row.quantity),
            backgroundColor: [
              "rgba(255, 99, 132, 0.5)",
              "rgba(54, 162, 235, 0.6)",
              "rgba(255, 206, 86, 0.2)",
              "rgba(75, 192, 192, 0.5)",
              "rgba(153, 102, 255, 0.4)",
              "rgba(255, 159, 64, 0.7)",
            ],
            borderColor: [
              "rgba(255, 99, 132, 1)",
              "rgba(54, 162, 235, 1)",
              "rgba(255, 206, 86, 1)",
              "rgba(75, 192, 192, 1)",
              "rgba(153, 102, 255, 1)",
              "rgba(255, 159, 64, 1)",
            ],
            borderWidth: 1,
          },
        ],
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
          },
        },
        animation: true,
        responsive: true,
        plugins: {
          legend: {
            display: true,
          },
          tooltip: {
            enabled: true,
          },
        },
      },
    });
  }
});

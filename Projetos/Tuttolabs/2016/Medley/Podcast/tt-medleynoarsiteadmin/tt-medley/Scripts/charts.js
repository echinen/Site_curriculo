$(document).ready(function () {

    google.charts.load('current', { packages: ['corechart'] });
    //google.charts.setOnLoadCallback(drawChart);
    var active = "line";

    $('.tabs a').click(function (e) {
        e.preventDefault()
        $(this).tab('show')
    });
    $("#buscarValores").click(function () {
        var dataIni = $("#dataIni").val();
        var dataFim = $("#dataFim").val();
        if (active == "line") {
            google.charts.setOnLoadCallback(function () {
                drawChart(dataIni, dataFim);
            });
        } else if (active == "map") {
            google.charts.setOnLoadCallback(function () {
                drawRegionsMap(dataIni, dataFim);
            });
        } else if (active == "pie") {
            google.charts.setOnLoadCallback(function () {
                drawPieChart(dataIni, dataFim);
            });
        };
    });

    $("#mapChartTab").click(function () {
        active = "map";
        //$("#profile").append('<div id="regions_div"></div>');
        
    });

    $("#lineChartTab").click(function () {
        active = "line";
        // $("#home").append('<div id="lineChart"></div>');
        
    });

    $("#pieChartTab").click(function () {
        active = "pie";
       
    });

    /*$.ajax({
        beforeSend: function (xhr) { xhr.setRequestHeader('X-ZUMO-APPLICATION', 'cxmyfPgDBPNFEDTpVkbwPGKilpsOId44'); },
        url: "https://tt-medley-podcast.azure-mobile.net/api/mpc_visitantes_iphone?datainicio='2016-02-03'&datafim='2016-02-08'",
        contentType: "application/json",
        crossDomain: true
    }).success(function (callback) {
        console.log(callback)
    })*/

    function drawChart(dataIni, dataFim) {
        $.ajax({
            beforeSend: function (xhr) { xhr.setRequestHeader('X-ZUMO-APPLICATION', 'cxmyfPgDBPNFEDTpVkbwPGKilpsOId44'); },
            url: "https://tt-medley-podcast.azure-mobile.net/api/mpc_visitantes_dia?datainicio='" + dataIni + "'&datafim='" + dataFim + "'",
            contentType: "application/json"
        }).success(function (callback) {
            console.log(callback);
            // Define the chart to be drawn.
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Element');
            data.addColumn('number', 'Acessos');
            for (var i = 0; i < callback.length; i++) {
                data.addRows([
                    [callback[i].campo, callback[i].valor]
                ]);
            }

            var options = {
                legend: { position: 'bottom' }
            };
            var chart = new google.visualization.LineChart(document.getElementById('lineChart'));
            chart.draw(data, options);

        }).error(function () {
            console.log('erro');
        });
    }


    function drawPieChart(dataIni, dataFim) {
        $.ajax({
            beforeSend: function (xhr) { xhr.setRequestHeader('X-ZUMO-APPLICATION', 'cxmyfPgDBPNFEDTpVkbwPGKilpsOId44'); },
            url: "https://tt-medley-podcast.azure-mobile.net/api/mpc_visitantes_iphone?datainicio='" + dataIni + "'&datafim='" + dataFim + "'",
            contentType: "application/json"
        }).success(function (callback) {
            console.log(callback);
            var data = new google.visualization.DataTable();
            var opt = {
                is3D: true
            };
            data.addColumn('string', 'Modelo');
            data.addColumn('number', 'Acessos');

            for (var i = 0; i < callback.length; i++) {
                data.addRows([
                   [callback[i].campo, callback[i].valor]
                ]);
            }

            var chart = new google.visualization.PieChart(document.getElementById('pieChart'));

            chart.draw(data, opt);

        }).error(function () {
            console.log('erro');
        });
    }

    function drawRegionsMap(dataIni, dataFim) {
        var currentRegion;
        var dataArray = [['Country', 'Acessos']];
        $.ajax({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('X-ZUMO-APPLICATION', 'cxmyfPgDBPNFEDTpVkbwPGKilpsOId44');
            },
            url: "https://tt-medley-podcast.azure-mobile.net/api/mpc_visitantes_local?datainicio='" + dataIni + "'&datafim='" + dataFim + "'&local='pais'",
            contentType: "application/json",
            success: function (callback) {
                for (var i = 0 ; i < callback.length; i++) {
                    dataArray.push([callback[i].campo, callback[i].valor]);
                }

                var options = {};
                var mapChart = google.visualization.arrayToDataTable(dataArray);
                var chart = new google.visualization.GeoChart(document.getElementById('regions_div'));
                chart.draw(mapChart, options);

                var geochart = new google.visualization.GeoChart(
                    document.getElementById('regions_div'));
                var options = {
                    width: 556,
                    height: 347,
                    colorAxis: { colors: ['#acb2b9', '#2f3f4f'] } // Map Colors 
                };

                google.visualization.events.addListener(chart, 'regionClick', function (eventData) {
                    currentRegion = eventData.region;
                    options['region'] = eventData.region;
                    options['resolution'] = 'provinces';
                    options['displayMode'] = 'markers';
                    var data = [['City', 'Acessos']];
                    $.ajax({
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('X-ZUMO-APPLICATION', 'cxmyfPgDBPNFEDTpVkbwPGKilpsOId44');
                        },
                        url: "https://tt-medley-podcast.azure-mobile.net/api/mpc_visitantes_local?datainicio='" + dataIni + "'&datafim='" + dataFim + "'&local=cidade&pais=" + currentRegion,
                        contentType: "application/json",
                        success: function (callback) {
                            for (var i = 0 ; i < callback.length; i++) {
                                data.push([callback[i].campo, callback[i].valor]);
                            }
                            
                            var cityChart = google.visualization.arrayToDataTable(data);
                            chart.draw(cityChart, options);
                            
                        }
                    });
                });
            }
        });


    };
})
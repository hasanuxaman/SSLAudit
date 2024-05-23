var DashController = function () {

    var init = function (count) {

        if (count > 0) {
            $("#branchProfiles").modal("show");
            $('.draggable').draggable({
                handle: ".modal-header"
            });
        }


        $(document).ready(function () {
            $("#datepicker").datepicker({
                format: "yyyy",
                viewMode: "years",
                minViewMode: "years",
                autoclose: true
            });
        })

        //{
        //    title: 'Birthday',
        //        description: 'Brithday',
        //            start: moment('2023-07-01'),
        //                end: moment('2023-07-01'),
        //                    color: 'blue',
        //                        allDay: true
        //}
        //CKEDITOR.replace("ckEdit");
        //CKEDITOR.instances['ckEdit'].setData("<h1> data to render</h1>");
        //console.log(CKEDITOR.instances['ckEdit'].getData())
        //        $('#tinyMCE').tinymce({
        //            plugins: [
        //                'powerpaste code emoticons',
        //            ],
        //            height: 500,
        //            /* other settings... */
        //});

        getEvents();


        //drawOragnogram();



        $("#tBranchProfiles").on("dblclick", "td",
            function () {

                var branchId = $(this).closest("tr").find("td:eq(0)").text();
                var BranchName = $(this).closest("tr").find("td:eq(2)").text();

                var form = $('<form>', { method: 'POST' });
                var targetURL = '/Home/AssignBranch';
                form.attr('action', targetURL);

                form.append($('<input>', {
                    type: 'branchId',
                    name: 'branchId',
                    value: branchId
                }));
                form.append($('<input>', {
                    type: 'BranchName',
                    name: 'BranchName',
                    value: BranchName
                }));

                form.hide();

                $(".container-fluid").append(form);

                form.submit();
                form.remove();

            });





        const percentageCells = document.querySelectorAll("tbody tr td:nth-child(6)");
        percentageCells.forEach(cell => {
            const percentage = parseInt(cell.textContent);
            const circle = document.createElement("div");
            circle.className = "percentage-circle";

            const percentageText = document.createElement("span");
            percentageText.className = "percentage-text";
            percentageText.textContent = `${percentage}%`;

            circle.style.background = `conic-gradient(#20c997 ${percentage}%, transparent ${percentage}% 100%)`;


            cell.textContent = "";
            circle.appendChild(percentageText);
            cell.appendChild(circle);
        });

        
        const percentageCells2 = document.querySelectorAll("#uba tbody tr td:nth-child(6)");
        percentageCells2.forEach(cell => {
            const percentage = parseInt(cell.textContent);
            const circle = document.createElement("div");
            circle.className = "percentage-circle";

            const percentageText = document.createElement("span");
            percentageText.className = "percentage-text";
            percentageText.textContent = `${percentage}%`;

            circle.style.background = `conic-gradient(#2D9596 ${percentage}%, transparent ${percentage}% 100%)`;


            cell.textContent = "";
            circle.appendChild(percentageText);
            cell.appendChild(circle);
        });



        debugger;
        var Completed = $("#Completed").val();
        var Ongoing = $("#Ongoing").val();
        var Remaining = $("#Remaining").val();

        var UnPlanRemaining = $("#UnPlanRemaining").val();
        var UnPlanCompleted = $("#UnPlanCompleted").val();
        var UnPlanOngoing = $("#UnPlanOngoing").val();


        ////for porgressbar
        //const xValues = ["Completed", "Ongoing", "Remainging"];
        ////const yValues = [55, 49, 44];
        //const yValues = [Completed, Ongoing, Remaining];
        //const barColors = [
        //    "#b91d47",
        //    "#00aba9",
        //    "#2b5797"
        //];
        //new Chart("myChart", {
        //    type: "doughnut",
        //    data: {
        //        labels: xValues,
        //        datasets: [{
        //            backgroundColor: barColors,
        //            data: yValues
        //        }]
        //    },
        //    options: {
        //        title: {
        //            display: true,
        //            text: "GDIC Reports"
        //        }
        //    }
        //});


        //LastCode

        //const data = {
        //    labels: ['Completed', 'Ongoing','Remainging'],
        //    datasets: [
        //        {
        //            data: [Completed, Ongoing, Remaining],
        //            backgroundColor: ['#b91d47', '#00aba9','#2b5797'],
        //        },
        //        {
        //            data: [40, 60,90],
        //            backgroundColor: ['#b91d47', '#00aba9', '#2b5797'],
        //        }
        //    ],
        //};


        //new Chart(document.getElementById('combined-chart'), {
        //    type: 'doughnut',
        //    data: data,
        //});


        const data = {
            labels: ['Completed', 'Ongoing', 'Remaining'],
            datasets: [
                {
                    data: [Completed , Ongoing  , Remaining ],
                    //backgroundColor: ['#b91d47', '#00aba9', '#2b5797'],
                    backgroundColor: ['#00aba9', '#2b5797', '#b91d47'],
                    labels: ['Plan Completed', 'Plan Ongoing', 'Plan Remaining'], 
                },
                {
                    data: [UnPlanCompleted, UnPlanOngoing, UnPlanRemaining],
                    //backgroundColor: ['#b91d47', '#00aba9', '#2b5797'],
                    backgroundColor: ['#00aba9', '#2b5797', '#b91d47'],
                    labels: ['UnPlan Completed', 'UnPlan Ongoing', 'UnPlan Remaining'], 
                }
            ],
        };

        const options = {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        const datasetIndex = tooltipItem.datasetIndex;
                        const labelIndex = tooltipItem.index;
                        const label = data.datasets[datasetIndex].labels[labelIndex];
                        const value = data.datasets[datasetIndex].data[labelIndex];
                        //return label + ': ' + value;
                        return label + ': ' + value + '%';
                    }
                }
            },
            legend: {
                display: true,
            }
        };

        new Chart(document.getElementById('combined-chart'), {
            type: 'doughnut',
            data: data,
            options: options
        });




        //SecondPart
        

        $("#planDropdown").on("change", function () {
            var selectedValue = $(this).val();
            // Do something with the selected value, e.g., log it or perform other actions
            console.log("Selected Plan: " + selectedValue);

            // You can use the selected value in your logic here or trigger any other actions.


            $.ajax({
                url: '/Home/UpdateAuditComponents', // Replace with your actual controller and action
                type: 'POST', // or 'GET' depending on your server-side implementation
                data: { selectedValue: selectedValue },
                success: function (data) {
                    // Update the AuditComponentList in the HttpContext.Items
                    // You can also update the UI or perform any other actions here
                    console.log(data);
                },
                error: function (error) {
                    console.error('Error updating AuditComponentList:', error);
                }
            });




        });


    }


    //End of Init

    function drawOragnogram() {
        google.load("visualization", "1", { packages: ["orgchart"] });
        google.setOnLoadCallback(drawChart);
    }




    function drawChart() {
        $.ajax({
            type: "GET",
            url: "/Home/GetEmployees",
        })
            .done(function (result) {


                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Entity');
                data.addColumn('string', 'ParentEntity');
                data.addColumn('string', 'ToolTip');
                for (var i = 0; i < result.length; i++) {
                    var employeeId = result[i][0].toString();
                    var employeeName = result[i][1];
                    var designation = result[i][2];
                    var reportingManager = result[i][3] != 0 ? result[i][3].toString() : '';

                    var row =
                        [
                            [

                                {
                                    v: employeeId,
                                    f: employeeName + '<div>(<span>' + designation + '</span>)</div><img src = "/Images/' + employeeId + '.jpg" />'
                                }, reportingManager, designation
                            ]

                        ]




                    data.addRows(row);
                }

                var chart = new google.visualization.OrgChart($("#chart")[0]);
                chart.draw(data, { allowHtml: true });
            })
            .fail(function () {
                alert('failed');
            });

    }

    function getEvents() {

        var events = [];
        $.ajax({
            type: "GET",
            url: "/calender/GetEvents",

        })
            .done(function (data) {

                GenerateCalender(data);


            })
            .fail(function (fail) {
                alert('failed');
            });

    }


    function GenerateCalender(events) {
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            contentHeight: 400,
            defaultDate: new Date(),
            timeFormat: 'h(:mm)a',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay,agenda'
            },
            eventLimit: true,
            eventColor: '#378006',
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                //$('#myModal #eventTitle').text(calEvent.title);
                //var $description = $('<div/>');
                //$description.append($('<p/>').html('<b>Start:</b>' + calEvent.start.format("DD-MMM-YYYY HH:mm a")));
                //if (calEvent.end != null) {
                //    $description.append($('<p/>').html('<b>End:</b>' + calEvent.end.format("DD-MMM-YYYY HH:mm a")));
                //}
                //$description.append($('<p/>').html('<b>Description:</b>' + calEvent.description));
                //$('#myModal #pDetails').empty().html($description);

                //$('#myModal').modal();


            }
        })
    }

    return {
        init: init
    }

}();
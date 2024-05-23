
var ReportController = function () {
    var init = function () {

        $('#printButton').on('click', function () {



            debugger;
            var gdImage = new Image();
            gdImage.src = "/images/d.png";
            gdImage.alt = "GDIC Image";
            gdImage.className = "gd-image";
            gdImage.width = 200;
            gdImage.height = 100;

            var gdImage2 = new Image();
            gdImage2.src = "/images/d.png";
            gdImage2.alt = "GDIC Image";
            gdImage2.className = "gd-image";
            gdImage2.width = 200;
            gdImage2.height = 100;

            var gdImageClone = gdImage.cloneNode(true);

            gdImage.onload = function () {

                $('#gdLabelContainer').prepend(gdImage);
                //$('#gdLabelContainer2').prepend(gdImage2);

                $('a').not('.no-print a').each(function () {
                    var pTag = $('<b>').html($(this).html());
                    $(this).replaceWith(pTag);
                });

                window.print();
                gdImage.remove();
                //gdImage2.remove();

                debugger;
                //var container = document.createElement('div');
                //container.classList.add('print-container');
                //container.appendChild(gdImageClone);
                //document.body.insertBefore(container, document.body.firstChild);
                //window.print();
                //document.body.removeChild(container);

            };
        });



    }

    return {
        init: init
    }

}();









$(document).ready(function () {

	$('#AutoScroll').change(function () {
		if ($('#AutoScroll').prop('checked')) {
			setInterval(function () {

				// Time to scroll to bottom
				$('html, body').animate({
					scrollTop: 0
				}, 2000);

				// Scroll to top
				setTimeout(function () {
					$('html, body').animate({
						scrollTop: $(document).height()
					}, 16000);
				}, 2000); //call every 2000 miliseconds
			}, 2000);
		}
	});

	$('i').click(function () {
		$(this).parents('div.col-sm-4').hide();
	});
})
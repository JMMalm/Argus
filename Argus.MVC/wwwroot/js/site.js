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

	$('#UnhideLink').click(function () {
		$('div.col-sm-4[data-hidden="true"').fadeIn();
	});

	$('h5 > i').click(function () {
		$(this).parents('div.col-sm-4').attr('data-hidden', true).fadeOut();
	});
})

var interval = null;
function enableAutoRefresh() {
	interval = setInterval(refreshData, 60000); // 60 seconds
}

function disableAutoRefresh() {
	clearInterval(interval);
}

function refreshData() {
	// Place-holder functionality for testing.
	alert('Refresh called!');
}
var refreshInterval = null;
var scrollInterval = null;

$(document).ready(function () {

	// "e.StopPropagation" will keep the dropdown open after clicking either checkbox.
	// Click outside the dropdown to close it.
	$('#AutoScrollCheckbox').change(function () {
		if ($('#AutoScrollCheckbox').prop('checked')) {
			scrollInterval = setInterval(autoScroll, 2000);
		}
		else {
			clearInterval(scrollInterval);
		}
	}).parents('.dropdown-menu').on("click.bs.dropdown", function (e) {
		e.stopPropagation();
	});

	$('#AutoRefreshCheckbox').change(function () {
		if ($('#AutoRefreshCheckbox').prop('checked')) {
			refreshInterval = setInterval(refreshData, 60000); // 60 seconds
		}
		else {
			clearInterval(refreshInterval);
		}
	}).parents('.dropdown-menu').on("click.bs.dropdown", function (e) {
		e.stopPropagation();
	});

	$('#UnhideLink').click(function () {
		$('div.col-sm-4[data-hidden="true"').fadeIn();
	});

	$('h5 > i').click(function () {
		$(this).parents('div.col-sm-4').attr('data-hidden', true).fadeOut();
	});
})

function refreshData() {
	// Place-holder functionality for testing.
	alert('Refresh called!');
}

function autoScroll() {
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
}
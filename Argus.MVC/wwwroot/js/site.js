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

	$('#UnhideCheckbox').change(function () {
		if ($('#UnhideCheckbox').prop('checked')) {
			$('div.col-sm-4[data-hidden="true"]')
				.attr('data-hidden', false)
				.fadeIn();
		}
		else {
			$('div.col-sm-4[data-hidden="false"]')
				.attr('data-hidden', true)
				.fadeOut();
		}
	});

	$('h5 > i').click(function () {
		$(this).parents('div.col-sm-4').attr('data-hidden', true).fadeOut();
		$('#UnhideCheckbox').prop('checked', false)
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

function GetApplicationUpdates(isTest = false) {
	// data-id
	var ids = [];
	$('div.col-sm-4:not([data-hidden="true"').each(function () {
		var id = $(this).attr('data-id');
		if (id) {
			ids.push(id);
		}
	});

	$.ajax({
		type: 'GET',
		contentType: 'application/json',
		data: { 'applicationIds': ids, 'isTest': isTest },
		dataType: "json",
		traditional: true,
		url: '/Home/GetApplicationUpdates',
		success: function (data) {
			$.each(data, function (index, data) {
				updateApplication(data);
			});
		}
	});
}

function updateApplication(application) {
	if (!application) {
		console.warn('Null application passed for update.');
	};

	var cardColor = 'bg-success';
	var cardTextColor = 'text-white';
	var $applicationCard = $('div.col-sm-4[data-id=' + application.id + ']');
	$applicationCard.find('button > span').text(application.issueCount);

	if (application.hasUrgentPriority || application.issueCount >= 3) {
		cardColor = 'bg-danger';
	}
	else if (!application.hasUrgentPriority && application.issueCount != 0) {
		cardColor = 'bg-warning';
		cardTextColor = 'text-dark';
	}

	$applicationCard
		.find("[class*='bg-']")
		.removeClass('bg-success bg-warning bg-danger text-light text-dark')
		.addClass('' + cardColor + ' ' + cardTextColor + '');

	// Not quite working right:
	//$applicationCard.('h5').removeClass(function (index, css) {
	//	return (css.match(/(^|\s)bg-\S+/g) || []).join(' ');
	//});
}
var refreshInterval = null;
var scrollInterval = null;

$(document).ready(function () {

	if (isAfterHours()) {
		$('#AfterHoursAlert').fadeIn();
	}
	else {
		$('#AfterHoursAlert').fadeOut();
	}

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
			refreshInterval = setInterval(getApplicationUpdates, 120000); // 120 seconds
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

	$('#SaveCheckbox').change(function () {
		if ($('#SaveCheckbox').prop('checked')) {
			saveSettings();
		}
		else {
			resetSettings();
		}
	});

	$('h5 > i').click(function () {
		$(this).parents('div.col-sm-4').attr('data-hidden', true).fadeOut();
		$('#UnhideCheckbox').prop('checked', false)
	});
})

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

function getApplicationUpdates(isTest = false) {

	if (isAfterHours()) {
		$('#AfterHoursAlert').fadeIn();
		return;
	}
	else {
		$('#AfterHoursAlert').fadeOut();
	}

	$('#loadingModal').modal('show');

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
		},
		error: function (xhr, error) {
			console.error(xhr.status + ': There was a problem on the server.');
		},
		complete: function () {
			$('#StatusDateTime').text(new Date($.now()).toLocaleString());
			setTimeout(function () {
				$('#loadingModal').modal('hide');
			}, 1000);
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
		.fadeIn()
		.removeClass('bg-success bg-warning bg-danger text-light text-dark')
		.addClass('' + cardColor + ' ' + cardTextColor + '');
}

function showReportIssueModal() {
	$('#reportIssueModal').modal('show');
}

function isAfterHours() {
	var localHour = new Date($.now()).getHours();
	return (localHour >= 18 || localHour < 8);
}

function saveSettings() {
	var hiddenIds = getIdsOfHiddenApplications();
	localStorage.setItem('hiddenApps', hiddenIds);
	localStorage.setItem('saveSettings', $('#SaveCheckbox').prop('checked'));
}

function getIdsOfHiddenApplications() {
	var ids = [];
	$('div.col-sm-4[data-hidden="true"]').each(function () {
		var id = $(this).attr('data-id');
		if (id) {
			ids.push(id);
		}
	});
	return ids;
}

function checkSavedSettings() {
	if (localStorage.getItem('saveSettings') === true) {
		$('#SaveCheckbox').prop('checked', true);

		var hiddenIds = localStorage.getItem('hiddenApps');
		if (hiddenIds) {
			hiddenIds.split(',').forEach(function (v) {
				$('div.col-sm-4[data-id="' + v + '"]')
					.attr('data-hidden', true)
					.fadeOut();
			});

			var $unhideCheckbox = $('#UnhideCheckbox');
			if ($unhideCheckbox.prop('checked')) {
				$unhideCheckbox.prop('checked', false);
			}
		}
	}
}
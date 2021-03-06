﻿var refreshInterval = null;
var scrollInterval = null;
var sortOption = "0";

$(document).ready(function () {

	if (isAfterHours()) {
		$('#AfterHoursAlert').fadeIn();
	}
	else {
		$('#AfterHoursAlert').fadeOut();
	}

	sortOption = $('#SortSelectionInputGroup').val();

	$('#settingsLink').click(function () {
		$('#settingsModal').modal('show');
	})

	$("#settingsModal").on("hidden.bs.modal", function () {
		var selectedSortOption = $('#SortSelectionInputGroup').val();
		if (sortOption !== selectedSortOption) {
			document.location.href = location.origin + '?sortOption=' + selectedSortOption;
		}
	});

	$('#AutoScrollCheckbox').change(function () {
		if ($('#AutoScrollCheckbox').prop('checked')) {
			scrollInterval = setInterval(autoScroll, 2000);
		}
		else {
			clearInterval(scrollInterval);
		}
	})

	$('#AutoRefreshCheckbox').change(function () {
		if ($('#AutoRefreshCheckbox').prop('checked')) {
			refreshInterval = setInterval(getApplicationUpdates, 120000); // 120 seconds
		}
		else {
			clearInterval(refreshInterval);
		}
	})

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

	checkSavedSettings();
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

	if (isAfterHours() && !isTest) {
		$('#AfterHoursAlert').fadeIn();
		console.log('After hours detected - skipping update request.')
		return;
	}
	else {
		$('#AfterHoursAlert').fadeOut();
	}

	$('#loadingModal').modal('show');

	var ids = getIdArray('div.col-sm-4:not([data-hidden="true"');
	var sortOption = $('#SortSelectionInputGroup').val();

	$.ajax({
		type: 'GET',
		contentType: 'application/json',
		data: {
			'applicationIds': ids,
			'sortOption': sortOption,
			'isTest': isTest
		},
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

	if (application.hasUrgentPriority && !$applicationCard.find('i.pulse').length) {
		$applicationCard
			.find('i')
			.before('<i class="fa fa-exclamation-triangle pulse" aria-hidden="true" title="High priority issue(s) have been reported."></i>');
	}
	else if (!application.hasUrgentPriority && $applicationCard.find('i.pulse').length) {
		$applicationCard.find('i.pulse').remove();
	}
}

function showReportIssueModal() {
	$('#reportIssueModal').modal('show');
}

function isAfterHours() {
	var localHour = new Date($.now()).getHours();
	return (localHour >= 18 || localHour < 8);
}

function saveSettings() {
	var hiddenIds = getIdArray('div.col-sm-4[data-hidden]');
	localStorage.setItem('hiddenApps', hiddenIds);
	localStorage.setItem('showHidden', $('#UnhideCheckbox').prop('checked'));
	localStorage.setItem('sortOption', $('#SortSelectionInputGroup').val());
	localStorage.setItem('saveSettings', true);

	localStorage.setItem('enabledAutoRefresh', $('#AutoRefreshCheckbox').prop('checked'));
	localStorage.setItem('enabledAutoScroll', $('#AutoScrollCheckbox').prop('checked'));

	$('#SaveSettingsAlert').fadeIn();
	setTimeout(function () {
		$("#SaveSettingsAlert").fadeOut(2000);
	}, 5000);
}

function checkSavedSettings() {
	if (localStorage.getItem('saveSettings')) {

		var hiddenIds = localStorage.getItem('hiddenApps');
		var showHidden = (localStorage.getItem('showHidden') == 'true');
		if (hiddenIds) {
			hiddenIds.split(',').forEach(function (v) {
				$('div.col-sm-4[data-id="' + v + '"]')
					.attr('data-hidden', showHidden);
			});

			$('#UnhideCheckbox')
				.prop('checked', showHidden)
				.change();
		}

		if (localStorage.getItem('enabledAutoRefresh') == 'true') {
			$('#AutoRefreshCheckbox')
				.prop('checked', true)
				.change();
		}

		if (localStorage.getItem('enabledAutoScroll') == 'true') {
			$('#AutoScrollCheckbox')
				.prop('checked', true)
				.change();
		}
	}
}

function resetSettings() {
	localStorage.removeItem('enabledAutoRefresh');
	$('#AutoRefreshCheckbox')
		.prop('checked', false)
		.change();
	localStorage.removeItem('enabledAutoScroll');
	$('#AutoScrollCheckbox')
		.prop('checked', false)
		.change();
	localStorage.removeItem('hiddenApps');
	localStorage.removeItem('showHidden');
	$('div.col-sm-4[data-hidden]')
		.removeAttr('data-hidden')
		.fadeIn();
	$('#UnhideCheckbox')
		.prop('checked', false);
	localStorage.removeItem('sortOption');
	$('#SortSelectionInputGroup').val(0);
	localStorage.removeItem('saveSettings');

	$('#ResetSettingsAlert').fadeIn();
	setTimeout(function () {
		$("#ResetSettingsAlert").fadeOut(2000);
	}, 5000);
}

function checkSortOption() {
	var sortOption = localStorage.getItem('sortOption');
	if (sortOption && window.location.href.indexOf("sortOption") == -1) {
		window.location.href = location.origin + '?sortOption=' + sortOption;
	}
}

function getIdArray(selector) {
	var ids = [];
	$(selector).each(function () {
		var id = $(this).attr('data-id');
		if (id) {
			ids.push(id);
		}
	});

	return ids;
}
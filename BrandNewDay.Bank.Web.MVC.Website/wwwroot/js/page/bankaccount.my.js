const deposit_modal = document.getElementById('deposit-modal')
const transfer_modal = document.getElementById('transfer-modal')

function initDepositModal() {
    $('#btn-open-deposit-modal').unbind('click').click(function () {
        $('#deposit-modal').modal('show');
    });

    $('.btn-close-modal').unbind('click').click(function () {
        $('.modal').modal('hide');
    });

    deposit_modal.addEventListener('shown.bs.modal', event => {

        const modalTitle = deposit_modal.querySelector('.modal-title');
        const modalBody = $('#deposit-modal .modal-body');

        $('#txt-deposit-amount').trigger('focus');

        $('#txt-deposit-amount').unbind('keyup').keyup(function () {
            var amt = parseFloat($(this).val());
            var fee = amt * (0.1 / 100);

            $('#txt-deposit-fee').val(fee.toFixed(3));
        });

    });

    deposit_modal.addEventListener('hidden.bs.modal', event => {
        const button = event.relatedTarget

        $('#txt-deposit-amount, #txt-deposit-fee').val('0.000');
    })
}

function initTransferModal() {
    $('#btn-open-transfer-modal').unbind('click').click(function () {
        $('#transfer-modal').modal('show');
    });

    $('.btn-close-modal').unbind('click').click(function () {
        $('.modal').modal('hide');
    });

    transfer_modal.addEventListener('shown.bs.modal', event => {

        const modalTitle = transfer_modal.querySelector('.modal-title');
        const modalBody = $('#transfer-modal .modal-body');

        $('#txt-transfer-iban').trigger('focus');
    });

    transfer_modal.addEventListener('hidden.bs.modal', event => {
        const button = event.relatedTarget
        $('#txt-transfer-iban').val('');
        $('#txt-transfer-amount').val('0.000');
    })
}


function initConfirmDepositButton() {
    $('#btn-confirm-deposit').unbind('click').click(function () {
        var button = $(this);
        button.attr('disabled', true);

        var amount = $('#txt-deposit-amount').val().replace(',', '');

        var params = { amount: amount };

        $.post('/BankAccount/Deposit', params, function (response) {
            if (response.result) {
                //update balance
                $('.acc-balance').html(response.updateBalance);
                $('#txt-current-balance, #txt-tf-current-balance').val(response.updateBalance);
                $('.modal').modal('hide');
            }

            button.removeAttr('disabled');
        });
    });
}

function initConfirmTransferButton() {
    $('#btn-confirm-transfer').unbind('click').click(function () {
        var button = $(this);
        button.attr('disabled', true);

        var amount = $('#txt-transfer-amount').val().replace(',', '');
        var to_account = $('#txt-transfer-iban').val();

        var params = { toAccount: to_account, amount: amount };

        $.post('/BankAccount/Transfer', params, function (response) {
            if (response.result) {
                //update balance
                $('.acc-balance').html(response.updateBalance);
                $('#txt-current-balance, #txt-tf-current-balance').val(response.updateBalance);
                $('.modal').modal('hide');
            }

            button.removeAttr('disabled');
        });
    });
}

$(document).ready(function () {
    initDepositModal();
    initTransferModal();
    initConfirmDepositButton();
    initConfirmTransferButton();
});
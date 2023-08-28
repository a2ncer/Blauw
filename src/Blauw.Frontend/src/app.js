const { get } = require("http");

(function () {

    const accountsServiceUri = process.env.ACCOUNTS_SERVICE_URI;
    const transactionsServiceUri = process.env.TRANSACTIONS_SERVICE_URI;

    document.addEventListener("DOMContentLoaded", function () {
        const customerDropdown = document.getElementById("customerDropdown");
        const currencyDropdown = document.getElementById("currencyDropdown");
        const balanceInput = document.getElementById("balanceInput");
        const createAccountBtn = document.getElementById("createAccountBtn");
        const accountRows = document.getElementById('accountList');
        const accountBalanceInput = document.getElementById('accountBalanceInput');
        const depositAccountBtn = document.getElementById("depositAccountBtn");
        const withdrawAccountBtn = document.getElementById("withdrawAccountBtn");

        const accountBalance = document.getElementById('accountBalance');
        const accountCurrency = document.getElementById('accountCurrency');
        const accountId = document.getElementById('accountId');
        const customerId = document.getElementById('customerId');

        var currentAccountId = null;

        withdrawAccountBtn.addEventListener("click", function () {
            const selectedAccount = currentAccountId;
            const amount = -Math.abs(parseFloat(accountBalanceInput.value));

            updateBalance(selectedAccount, amount);
        });

        depositAccountBtn.addEventListener("click", function () {
            const selectedAccount = currentAccountId;
            const amount = parseFloat(accountBalanceInput.value);

            updateBalance(selectedAccount, amount);
        });

        function updateBalance(selectedAccount, amount) {
            
            if (selectedAccount == null) {
                console.log("No account selected");
                return;
            }

            if (isNaN(amount)) {
                console.log("Invalid amount : " + amount);
                return;
            }

            if (amount == 0) {
                console.log("Amount is zero");
                return;
            }

            console.log("Updating balance for account: " + selectedAccount + " with amount: " + amount);
            
            const requestBody = {
                amount: amount
            };

            fetch(`${accountsServiceUri}/accounts/${selectedAccount}`, {

                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            })
                .then(response => response.json())
                .then(data => {
                    console.log("Account withdraw:", data);
                    balanceInput.value = ""; // Clear input field
                    // Logic to update the account list or UI as needed
                    getAccountList();
                    getTransactionHistory();
                })
                .catch(error => {
                    console.error("Error withdraw account:", error);
                });
        }

        
        accountRows.addEventListener('click', function (e) {
            console.log(e.target.parentElement.firstChild.textContent);
            const selectedAccountId = e.target.parentElement.firstChild.textContent;
            const selectedCustomerId = e.target.parentElement.children[1].textContent;
            const selectedAccountBalance = e.target.parentElement.children[2].textContent;
            const selectedAccountCurrency = e.target.parentElement.children[3].textContent;

            currentAccountId = selectedAccountId;

            updateCurrentAccountDetails(selectedAccountId, selectedCustomerId, selectedAccountBalance, selectedAccountCurrency);

            // Call method that fetches updated transaction history 
            getTransactionHistory();

        });

        // Add an event listener for the 'change' event
        customerDropdown.addEventListener('change', function () {

            currentAccountId = null;
            clearCurrentAccountDetails();
            getAccountList();
        });

        // Trigger the 'change' event for the default selected option
        customerDropdown.dispatchEvent(new Event('change'));

        createAccountBtn.addEventListener("click", function () {
            const selectedCustomer = customerDropdown.value;
            const selectedCurrency = currencyDropdown.value;
            const initialBalance = parseFloat(balanceInput.value);

            const requestBody = {
                customerId: selectedCustomer,
                currency: selectedCurrency,
                balance: initialBalance
            };

            fetch(`${accountsServiceUri}/accounts`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(requestBody)
            })
                .then(response => response.json())
                .then(data => {
                    console.log("Account created:", data);
                    balanceInput.value = ""; // Clear input field
                    // Logic to update the account list or UI as needed
                })
                .catch(error => {
                    console.error("Error creating account:", error);
                });
        });

        function updateCurrentAccountDetails(selectedAccountId, selectedCustomerId, selectedAccountBalance, selectedAccountCurrency) {
            // Update account details in the view
            accountBalance.innerText = `Balance: ${selectedAccountBalance}`;
            accountCurrency.innerText = `Currency: ${selectedAccountCurrency}`;
            accountId.innerText = `Account ID: ${selectedAccountId}`;
            customerId.innerText = `Customer ID: ${selectedCustomerId}`;
        }

        function clearCurrentAccountDetails() {
            // Update account details in the view
            accountBalance.innerText = "";
            accountCurrency.innerText = "";
            accountId.innerText = "";
            customerId.innerText = "";
        }

        function getAccountList() {
            const selectedCustomerId = customerDropdown.value;

            console.log("Fetching account list for customer: " + selectedCustomerId);

            // Fetch account details
            fetch(`${accountsServiceUri}/accounts?customerId=${selectedCustomerId}`)
                .then(response => response.json())
                .then(accountDetails => {
                    const accountList = document.getElementById("accountList");
                    accountList.innerHTML = "<table></table>"; // Clear existing accounts
                    accountDetails.data.forEach(account => {
                        const accountItem = document.createElement("tr");
                        accountItem.innerHTML = `<td>${account.accountId}</td><td>${account.customerId}</td><td>${account.balance}</td><td>${account.currency}</td><td>${account.status}</td>`;
                        accountList.appendChild(accountItem);
                    });
                })
                .catch(error => {
                    console.error("Error fetching account details:", error);
                });
        }

        function getTransactionHistory() {

            if (currentAccountId == null) {
                // clear transaction history
                console.log("No account selected");
                const transactionList = document.getElementById("transactionList");
                transactionList.innerHTML = ""; // Clear existing transactions
                return;
            }

            console.log("Fetching transaction history for account: " + currentAccountId);

            // Fetch updated transaction history
            fetch(`${transactionsServiceUri}/transactions?accountId=${currentAccountId}`)
                .then(response => response.json())
                .then(transactions => {
                    // Update transaction history in the view
                    const transactionList = document.getElementById("transactionList");
                    transactionList.innerHTML = ""; // Clear existing transactions

                    transactions.data.forEach(transaction => {
                        const transactionItem = document.createElement("tr");
                        transactionItem.innerHTML = `<td>${transaction.id}</td><td>${transaction.accountId}</td><td>${transaction.createdAt}</td><td>${transaction.amount}</td><td>${transaction.currency}</td><td>${transaction.status}</td>`;

                        transactionList.appendChild(transactionItem);
                    });
                })
                .catch(error => {
                    console.error("Error fetching transaction history:", error);
                });
        }


        // Periodically update account data and transactions
        setInterval(function () {
            getAccountList();
            getTransactionHistory();
        }, 5000); // Update every 5 seconds
    });
})();
# TransactionService.py
import random
import time
import threading

class ExternalPaymentAPI:
    @staticmethod
    def send_payment_request(user_id, amount):
        print(f"[API] Sending payment request for user {user_id}, amount {amount}")
        time.sleep(1)  # Simulating network delay
        if amount <= 100:  # Simulated condition for approval
            print(f"[API] Payment approved for user {user_id}")
            return "Payment successful"
        else:
            print(f"[API] Payment declined for user {user_id}")
            return "Payment declined"
			
class TransactionService:
    def __init__(self, payment_gateway, notification_service):
        self.payment_gateway = payment_gateway
        self.notification_service = notification_service

    def initiate_transaction(self, user_id, amount):
        transaction_id = self.generate_transaction_id()
        print(f"[TS] Initiating transaction for user {user_id} with amount {amount}")
        try:
            payment_status = self.payment_gateway.process_payment(user_id, amount, transaction_id)
            if payment_status == "Payment successful":
                self.notification_service.send_notification(user_id, payment_status)
            else:
                raise RuntimeError("Payment processing error.")
        except Exception as e:
            print(f"[TS] Error processing transaction for user {user_id}: {str(e)}")
            self.notification_service.send_notification(user_id, None)
        return transaction_id  # Return the transaction ID for potential refund

    def check_transaction_history(self, user_id):
        print(f"[TS] Checking transaction history for user {user_id}")
        return [{"amount": 50, "status": "completed"}, {"amount": 150, "status": "failed"}]

    def generate_transaction_id(self):
        transaction_id = f"TRANS-{random.randint(1000, 9999)}"
        print(f"[TS] Generated transaction ID: {transaction_id}")
        return transaction_id

class PaymentGateway:
      def process_payment(self, user_id, amount):
        print(f"[PG] Processing payment for user {user_id}, amount {amount}")
        approval_status = ExternalPaymentAPI.send_payment_request(user_id, amount)
        return approval_status

    def validate_payment_details(self, user_id, amount):
        print(f"[PG] Validating payment details for user {user_id}, amount {amount}")
        return True

    def refund_payment(self, transaction_id):
        print(f"[PG] Refunding payment for transaction {transaction_id}")
        approval_status = ExternalPaymentAPI.send_payment_request(user_id, amount)
        return approval_status

class NotificationService:
    def send_notification(self, user_id, message):
        if message is None:
            raise ValueError("Notification message cannot be None.")
        print(f"[NS] Sending notification to user {user_id}: {message}")

    def log_notification(self, user_id, message):
        print(f"[NS] Logging notification for user {user_id}: {message}")

    def send_bulk_notifications(self, user_ids, message):
        for user_id in user_ids:
            self.send_notification(user_id, message)

    def schedule_notification(self, user_id, message, delay):
        print(f"[NS] Scheduling notification for user {user_id} in {delay} seconds.")
        time.sleep(delay)
        self.send_notification(user_id, message)

    def cancel_scheduled_notification(self, user_id):
        print(f"[NS] Canceling scheduled notification for user {user_id}.")

    def get_notification_status(self, user_id):
        print(f"[NS] Getting notification status for user {user_id}.")
        return "Notification status retrieved."

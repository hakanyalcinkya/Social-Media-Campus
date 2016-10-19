<?php

// Define some constants
define( "RECIPIENT_NAME", "YOUR NAME" );
define( "RECIPIENT_EMAIL", "YOUR EMAIL" );
define( "EMAIL_SUBJECT", "Visitor Message" );

// Read the form values
$success = false;
$senderName = isset( $_POST['senderName'] ) ? $_POST['senderName'] : "";
$phoneNumber = isset( $_POST['phoneNumber'] ) ? $_POST['phoneNumber'] : "";
$senderEmail = isset( $_POST['senderEmail'] ) ? preg_replace( "/[^\.\-\_\@a-zA-Z0-9]/", "", $_POST['senderEmail'] ) : "";
$message = isset( $_POST['message'] ) ? preg_replace( "/(From:|To:|BCC:|CC:|Subject:|Content-Type:)/", "", $_POST['message'] ) : "";

$messagecontent = "\r\n Name:" . $senderName . "\r\n Phone Number:" . $phoneNumber . "\r\n Email:" . $senderEmail . "\r\n Message:" . $message;

$SpamErrorMessage = "<div class='contact-form-container'><p>No Websites URLs permitted.</p></div>";
if(preg_match("/http/i", "$senderName")) {echo "$SpamErrorMessage"; exit();}
if(preg_match("/http/i", "$phoneNumber")) {echo "$SpamErrorMessage"; exit();}
if(preg_match("/http/i", "$senderEmail")) {echo "$SpamErrorMessage"; exit();}
if(preg_match("/http/i", "$message")) {echo "$SpamErrorMessage"; exit();}

  $recipient = RECIPIENT_NAME . " <" . RECIPIENT_EMAIL . ">";
  $headers = "From: " . $senderName . " <" . $senderEmail . ">";
  $success = mail( $recipient, EMAIL_SUBJECT, $messagecontent, $headers );

?>
<html>
  <head>
    <title>Thank You!</title>
      <link href="css/contactform.css" rel="stylesheet" type="text/css">
  </head>
  <body>
  <div class="contact-form-container">
  <?php if ( $success ) echo "<p>Thanks for sending your message! I'll get back to you shortly.</p>" ?>
  <?php if ( !$success ) echo "<p>There was a problem sending your message. Please try again.</p>" ?>
  <p>Click your browser's Back button to return to the page.</p>
  </div>
  </body>
</html>



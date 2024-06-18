void setup() {
    pinMode( 6 , OUTPUT);  // Must be a PWM pin
  analogWrite( 6 , 0 );  // 60% duty cycle
  Serial.begin(9600);
}

void loop() {
int x;

//for( x = 0 ; x < 154 ; x+=1 ) {
//Serial.println(x);
analogWrite( 6 , 255 );
delay(300);
//}

analogWrite( 6 , 0 );
Serial.println("--------------OFF");
delay(900);
}

#include <UnityRobot.h>
#include <ADCModule.h>
#include <DigitalModule.h>

ADCModule adc0(0, A0); // portA 0
ADCModule adc1(1, A1); // portA 1
ADCModule adc2(2, A2); // portA 2
ADCModule adc3(3, A3); // portA 3
ADCModule adc4(4, A4); // portA 4
ADCModule adc5(5, A5); // portA 5
ADCModule adc6(6, A6); // portA 6
ADCModule adc7(7, A7); // portA 7

DigitalModule digital0(8, 8); // portD 0
DigitalModule digital1(9, 9); // portD 1
DigitalModule digital2(10, 10); // portD 2
DigitalModule digital3(11, 11); // portD 3
DigitalModule digital4(12, 12); // portD 4
DigitalModule digital5(13, 13); // portD 5
DigitalModule digital6(14, 14); // portD 6
DigitalModule digital7(15, 15); // portD 7

DigitalModule digital8(16, 16); // portD 8
DigitalModule digital9(17, 17); // portD 9
DigitalModule digital10(18, 18); // portD 10
DigitalModule digital11(19, 19); // portD 11
DigitalModule digital12(20, 20); // portD 12
DigitalModule digital13(21, 21); // portD 13
DigitalModule digital14(22, 22); // portD 14
DigitalModule digital15(23, 23); // portD 15

void OnUpdate(byte id)
{
  digital0.update(id);
  digital1.update(id);
  digital2.update(id);
  digital3.update(id);
  digital4.update(id);
  digital5.update(id);
  digital6.update(id);
  digital7.update(id);
  digital8.update(id);
  digital9.update(id);
  digital10.update(id);
  digital11.update(id);
  digital12.update(id);
  digital13.update(id);
  digital14.update(id);
  digital15.update(id);
}

// When recieved end of update
void OnAction(void)
{
  //TODO: Synchronizing module's action
  digital0.action();
  digital1.action();
  digital2.action();
  digital3.action();
  digital4.action();
  digital5.action();
  digital6.action();
  digital7.action();
  digital8.action();
  digital9.action();
  digital10.action();
  digital11.action();
  digital12.action();
  digital13.action();
  digital14.action();
  digital15.action();
}

// When recieved start of connection
void OnStart(void)
{
  //TODO: Initialize argument of module
  digital0.reset();
  digital1.reset();
  digital2.reset();
  digital3.reset();
  digital4.reset();
  digital5.reset();
  digital6.reset();
  digital7.reset();
  digital8.reset();
  digital9.reset();
  digital10.reset();
  digital11.reset();
  digital12.reset();
  digital13.reset();
  digital14.reset();
  digital15.reset();
}

// When recieved exit of connection
void OnExit(void)
{
  //TODO: Initialize argument of module
}

void OnReady(void)
{
  adc0.flush();
  adc1.flush();
  adc2.flush();
  adc3.flush();
  adc4.flush();
  adc5.flush();
  adc6.flush();
  adc7.flush();
  
  digital0.flush();
  digital1.flush();
  digital2.flush();
  digital3.flush();
  digital4.flush();
  digital5.flush();
  digital6.flush();
  digital7.flush();
  digital8.flush();
  digital9.flush();
  digital10.flush();
  digital11.flush();
  digital12.flush();
  digital13.flush();
  digital14.flush();
  digital15.flush();
}

void setup()
{
  UnityRobot.attach(CMD_UPDATE, OnUpdate);
  UnityRobot.attach(CMD_ACTION, OnAction);
  UnityRobot.attach(CMD_START, OnStart);
  UnityRobot.attach(CMD_EXIT, OnExit);
  UnityRobot.attach(CMD_READY, OnReady);
  UnityRobot.begin(57600);
}

void loop()
{
  UnityRobot.process();
}

# Elevator
* Runs under a background thread, which is created whenever a floor request is made and destroyed once there's no more floors to visit.
* Supports receiving new requests on its way to a destiny.
* The notification is done through events.
* The elevator starts on the floor 0, then the user must input the desired floor.

# Considerations
* For this specific task I could simply manage a list of events with the expected time that they should be fired, but I've decided to take a more "real-world" approach.
* I'd like to break the Elevator into sub-classes (Door and Engine), but I considered it was enough for a test.

## Parameters
* floors: The amount of floors on the building.
* height: The height of each floor in meters.
* speed: The elevator speed in m/s.
* delay: The time span between opening and closing the door in seconds.
* help: Displays the available parameters.

## Example
<code>Elevator --floors 5 --height=10 --speed=3 --delay=2</code>

## Dependencies
* [fluent-command-line-parser](https://github.com/fclp/fluent-command-line-parser): Parses the command-line arguments.

## Original Task
Предлагаем вам решить тестовое задание — написать программу «симулятор лифта».

Программа запускается из командной строки, в качестве параметров задается:

- кол-во этажей в подъезде — N (от 5 до 20);
- высота одного этажа;
- скорость лифта при движении в метрах в секунду (ускорением пренебрегаем, считаем, что когда лифт едет — он сразу едет с определенной скоростью);
- время между открытием и закрытием дверей.

После запуска программа должна постоянно ожидать ввода от пользователя и выводить действия лифта в реальном времени. События, которые нужно выводить:

- лифт проезжает некоторый этаж;
- лифт открыл двери;
- лифт закрыл двери.

Возможный ввод пользователя:

- вызов лифта на этаж из подъезда;
- нажать на кнопку этажа внутри лифта.

Считаем, что пользователь не может помешать лифту закрыть двери.

Все данные, которых не хватает в задаче, можно выбрать на свое усмотрение.
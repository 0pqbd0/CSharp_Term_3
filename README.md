
# Homeworks for С# course

[Parallel matrix multiplication](https://github.com/0pqbd0/CSharp_Term_3/tree/main/MatrixMultiplication)
<details>
<summary>Условие</summary>
Требуется реализовать параллельное умножение для плотных целочисленных матриц.
На входе программа получает файлы с матрицами (не обязательно квадратными), на выходе должен получиться файл, содержащий матрицу — их произведение. Сравнить скорость работы с последовательным вариантом в зависимости от размеров матриц.

Можно использовать только класс Thread для организации параллельной работы.
</details>

[Lazy calculation](https://github.com/0pqbd0/CSharp_Term_3/tree/main/Lazy)
<details>
<summary>Условие</summary>
Реализовать следующий интерфейс, представляющий ленивое вычисление:
`public interface ILazy<T> { T Get(); }`

Объект Lazy создаётся на основе вычисления (представляемого объектом `Func<T>`, который передаётся в конструктор, далее `supplier`)

* Первый вызов `Get()` вызывает `supplier` и возвращает результат.
* Повторные вызовы `Get()` возвращают тот же объект, что и первый вызов;
* Вычисление должно запускаться не более одного раза (то есть `supplier` после первого вызова не нужен и может быть удалён сборщиком мусора).

Интерфейс должен быть реализован двум способами:

* Простая версия с гарантией корректной работы в однопоточном режиме (без синхронизации);
* Гарантия корректной работы в многопоточном режиме. При этом она должна по возможности минимизировать число необходимых синхронизаций (если значение уже вычислено, не должно быть блокировок)
`supplier` вправе вернуть null;
* Библиотечным Lazy пользоваться, естественно, нельзя.
</details>

[MyThreadPool](https://github.com/0pqbd0/CSharp_Term_3/tree/main/MyThreadPool)
<details>
<summary>Условие</summary>
Реализовать простой пул задач с фиксированным числом потоков (число задается в конструкторе)
* При создании объекта MyThreadPool в нем должно начать работу n потоков
* У каждого потока есть два состояния: ожидание задачи / выполнение задачи
* Задача — вычисление некоторого значения, описывается в виде `Func<TResult>`
* При добавлении задачи, если в пуле есть ожидающий поток, то он должен приступить к ее исполнению. Иначе задача будет ожидать исполнения, пока не освободится какой-нибудь поток
* Задачи, принятые к исполнению, представлены в виде объектов интерфейса `IMyTask<TResult>`
* Метод Shutdown должен завершить работу потоков. Завершение работы коллаборативное, с использованием CancellationToken — уже запущенные задачи не прерываются, но новые задачи не принимаются на исполнение потоками из пула. Возможны два варианта решения — дать всем задачам, которые уже попали в очередь, досчитаться, либо выбросить исключение во все ожидающие завершения задачи потоки. Shutdown не должен возвращать управление, пока все потоки не остановились

IMyTask:
* Свойство IsCompleted возвращает true, если задача выполнена
* Свойство Result возвращает результат выполнения задачи. В случае, если соответствующая задаче функция завершилась с исключением, этот метод должен завершиться с исключением AggregateException, содержащим внутри себя исключение, вызвавшее проблему. Если результат еще не вычислен, метод ожидает его и возвращает полученное значение, блокируя вызвавший его поток
* Метод ContinueWith — принимает объект типа `Func<TResult, TNewResult>`, который может быть применен к результату данной задачи X и возвращает новую задачу Y, принятую к исполнению
Новая задача будет исполнена не ранее, чем завершится исходная. В качестве аргумента объекту Func будет передан результат исходной задачи, и все Y должны исполняться на общих основаниях (т.е. должны разделяться между потоками пула). Метод ContinueWith может быть вызван несколько раз. Метод ContinueWith не должен блокировать работу потока, если результат задачи X ещё не вычислен. ContinueWith должен быть согласован с Shutdown — принятая как ContinueWith задача должна либо досчитаться, либо бросить исключение ожидающему её потоку.
</details>

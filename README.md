# CPL_Converter
Проект для конвертирования файла pick and place сгенерированным программой ALtium Designer в файл Excel для заказа монтажа печатных плат на сайте JLCPCB.

![Внешний вид](Resources/MainForm.png)

## Требования

На компьютере должен быть установлен MS Excel.

## Настройка ALtium Designer

Чтобы сгенерировать pick and place файл со стороны Altium Designer необходимо добавить в outputjob файл проекта следующий элемент:

![Добавление CPL](Resources/AD_CPL_Add.png)

Далее нужно нажать ПКМ на созданный элемент и выбрать пункт Configure:

![Открыть конфигурацию](Resources/AD_CPL_Config.png)

Затем выполнить настройку элемента как на картинке:

![Rонфигурацию](Resources/AD_CPL_Settings.png)

Чтобы выгрузить файл pick and place нужно настроить каталог размещения файла и нажать Run:

![Генерация файла](Resources/AD_CPL_Run.png)

## Как использовать программу

### Способ 1. Запуск файла CPL_Converter.exe

После запуска приложения необходимо нажать кнопку "Загрузить файл" затем выбрать pick and place файл сгенерированный программой Altium Designer. После обработки откроется MS Excel с сгенерированными данными, которые затем нужно сохранить.

### Способ 2. Через конекстное меню pick and place файла

Нажать ПКМ на сгенерированный программой Altium Designer pick and place файл и выбрать элемент меню "Открыть с помощью"  затем выбрать программу CPL_Converter:

![Конекстное меню](Resources/ContextMenuUsing1.png)

![Выбор программы](Resources/ContextMenuUsing2.png)

Обработанный файл будет расположен в том же каталоге, где был входной файл:

![Результат](Resources/ContextMenuUsing3.png)

## Пример работы программы

Пример входного файла:

![Входной файл](Resources/InputFile_ex.png)

Сгенерированный выходной файл:

![Выходной файл](Resources/OutputFile_ex.png)
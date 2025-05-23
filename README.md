# `Make` for Sylux
Программа, предназначена для сборки ядра `Sylux`. Входит в комплект установки при выполнении установочного скрипта, который устанавливает необходимые модули для сборки, исходного кода `Sylux`. Установочный скрипт модулей необходим для облегчения исходного кода ядра `Sylux`.

# Сборка
Для сборки `Make` необходимо скачать дополнительную библиотеку из `NuGet` под названием `MultiAPI_Lib`. 
Чтобы собрать проект выполните один из скриптов, `build.bat` - В случае, если у вас стоит операционная система `Windows`, `build.sh` - Если `Linux`. В обоих вариантах требуется пакет разработчика `.net 9`.

# Конфигурационный файл
Файл конфигурации содержит структуру INI файла, с несколькими секторами и переменными. В секторе `[Compiler]` есть переменная `i686-elf-tools`, в ней необходимо указать путь до инструментов `i686-elf-tools`. Если вы их не установили или не знаете где они расположены, то выполните следующую команду: `sudo ./Make install_i686`, она в автоматическом порядке установит необходимые пакеты для сборки, инструменты компиляции, а также автоматически укажет значение до инструментов компиляции.

В секторе `[Debug]` есть переменная `qemu`, в ней необходимо указать путь до инструментов `qemu`, либо указать команду для выполнения. Например: `qemu-system-x86_64`. В первом случае вам будет необходимо правильно настроить конфигурацию сборки.

В секторе `[Output]` есть несколько переменных `bin` и `iso`. В переменной `bin` необходимо указать полный путь до выходного бинарного файла (Вместе с именем и расширением). Чтобы автоматизировать процесс сборки в iso файл укажите путь до папки `boot`, в которой будет распологаться директория `grub`. Например: `build/boot/sylux.bin`. В переменной `iso` необходимо указать полный путь до выходного ISO файла (Вместе с именем и расширением). Вы можете указать любой путь. Например: `sylux.iso`.

Сектор `[Vars]` отличается тем, что это основной класс для того, чтобы добавлять свои переменные, но как показывает практика вы можете делать это в любой секции. Как добавить переменную объясняется чуть ниже.

Сектора `[Build]` `[Run]` `[Clean]` в основном похожи друг на друга. Первый отвечает за события сборки, второй за события запуска проекта, третий за события очистки. Каждая из этих секций имеет следующую переменную: `Count`. Она устанавливает для каждой секции кол-во выполняемых указаний. Для добавления нового указания укажите новую переменную, и назовите её продолжением числового порядка. Например: У вас есть переменная `1`, значит после неё должна стоять переменная `2`, после этой переменной переменная `3` и так далее. Отсчёт переменных (указаний) начинается от `1`. В переменной `Count` указывайте кол-во указаний. Если у вас есть 3 переменные, то значение `Count` должно быть `3`. Вы можете нарушать порядок расположения переменных, но указания будут выполнятся в числовом порядке, от меньшего к большему. Например: У вас есть указания с идентификаторами `1`, `3`, `8`, `5`, выполняться они будут в следующей очерёдности: `1`, `3`, `5`, `8`. Обратите внимание, что в таком случае вам необходимо указать в переменной `Count` значение, максимального идентификатора переменных, в этом случае `8`. Чтобы создать указание введите идентификатор указания (Числовой номер), поставьте знак равно, а после введите команду. Например: `1=rm %iso%`. Также в указания можно встраивать переменные.

## Создание и использование переменных в конфигурационном файле
Для использования переменной вы можете использовать следующую конструкцию `%var%`. Вместо `var` нужно указать нужную вам переменную. Вставьте данную конструкцию в указания, расположенные в секторах `[Build]`, `[Run]`, `[Clean]`. Учтите, что данную конструкцию можно вставить только в указатели, её нельзя вставить в другие переменные или в переменную `Count`. Например, вы можете указать такую переменную: `%i686-elf-tools%` или `%bin%` и т.д.

Создать переменную тоже очень просто. Достаточно выбрать нужный сектор расположения, а после ввести идентификатор переменный и её значение. Основным сектором пользовательских переменных является `[Vars]`, но вам не запрещено создавать их в других секторах, таких как `[Compile]`, `[Output]` и т.д.. Пример создания переменной: `foo=foo_foo`, вы можете вставить данную переменную в указание: `1=%foo%`, в таком случае указание `1` будет равно `foo_foo`.

## Стандартные команды в указаниях
Вы также можете использовать стандартные команды в указаниях. К таким командам относятся: `echo`, `space`, `foreach`. `echo` - выводит текстовое сообщение в консоль. `space` - Создёт отступ в консоли. `foreach` - Создаёт цикл перебора файлов. Чтобы использовать стандартные команды в указаниях введите их первым словом. Например: `1=echo Hello, World!`, вывод в консоль: `Make: Hello, World!`. Например: `1=space`. Вы можете также комбинировать данные команды:
```
1=echo Hello, World!
2=space
3=echo Hello, User!
```
Вывод в консоль:
```
Make: Hello, World!

Make: Hello, User!
```
Указание `echo` поддерживает 1 аргумент - сообщение. Данная команда выводит строку на экран. Пример:
```
1=echo Hello, World!
```
Указание `space` не поддерживает аргументы. Данная команда отступает строку в терминале. Пример:
```
1=space
```
Указание `foreach` поддерживает 2 аругмента. 1-й аргумент должен быть равен `gcc` или `ld`, или `output`. `gcc` - Получает все файлы с расширением `.c` и выводит их в переменную `*`, без расширения, с которой можно взаимодействовать так: `%*%`. `ld` - Получает все файлы с расширением `.o` и выводит их в переменную `*`. Разница между `gcc` и `ld` в том, что `gcc` создаёт столько итераций сколько и файлов с расширением `.c` и выводит их в переменную `*`, а `ls`, в свою очередь, делает только одну итерацию, но копликтует все файлы в одну строку, с указанием расширения (`.o`). Аргумент `gcc` полезен в том случае, когда нужно скомпилировать файлы, например, у нас есть 3 файла: `kernel.c`, `window.c` и `Console.c`, мы можем ввести такую конфигурацию:
```
1=./%i686-elf-tools%/%gcc% -c kernel.c -o kernel.o -std=gnu99 -ffreestanding -O2 -Wall -Wextra
2=./%i686-elf-tools%/%gcc% -c window.c -o window.o -std=gnu99 -ffreestanding -O2 -Wall -Wextra
3=./%i686-elf-tools%/%gcc% -c Console.c -o Console.o -std=gnu99 -ffreestanding -O2 -Wall -Wextra
```
Но мы можем уместить весь этот код в одну строку:
```
1=foreach gcc ./%i686-elf-tools%/%gcc% -c %*%.c -o %*%.o -std=gnu99 -ffreestanding -O2 -Wall -Wextra
```
Так код будет выполняться также, но он уместился в одну строку, таким образом мы можем не указывать определённое название, код сам найдёт нужные файлы. 

Аргумент `ld` полезен в том случае, когда нужно отлинковать все файлы проекта. Например, допустим у нас есть все те же файлы и мы уже их скомпилировали в файлы с расширением `.o`, мы можем ввести все эти файлы вручную:
```
1=./%i686-elf-tools%/%gcc% -T %linker% -o %bin% -ffreestanding -O2 -nostdlib kernel.o window.o Console.o
```
Но мы можем использовать цикл foreach для сборки всех файлов в одну строку:
```
1=foreach ld ./%i686-elf-tools%/%gcc% -T %linker% -o %bin% -ffreestanding -O2 -nostdlib %*%
```
Так выполнится одна итерация, но при этом в переменной `*` будут все выходные файлы, с расширением `.o`, для линковки. Эти аргументы облегчат вам компиляцию и итоговую сборку. При выполнения любого из аргументов (`gcc`, `ls`, `output`) в итерации не будут входить файлы из папки `%i686-elf-tools%`, даже если они имеют расширение `.c` или `.o`.

Аргумент `output` - это аналог аргументу `gcc`, он делает тоже самое, но уже с другим расширением файла: `.o`. Данный аргумент можно использовать при очистке проекта:
```
1=foreach output rm %*%.o
```

# Стандартные аргументы и сборка ядра
В состав аргументов `Make` входят: `build`, `run`, `clean`, `version`, `install_i686`.

`build` или без аругментов - Автоматическая сборка ядра, взаимодействует с данными из конфигурационного файла, а точнее с указаниями для сборки.

`run` - Выполняет указания запуска ядра.

`clean` - Выполняет указания очистки.

`version` - Выводит текущую версию `Make`.

`install_i686` - Устанавливает необходимые пакеты для сборки ядра. 
using System;
using System.Collections.Generic;
using TSLab.Script.Helpers;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options; //чтобы можно было указывать атрибуты класса "кубиков"
using System.ComponentModel; //чтобы работало "Описание

namespace TSLab.TraidingLaboratory.Indicators
{
    #region параметры кубика

    //показывает в каком разделе кубиков будет находиться наш кубик
    [HandlerCategory("TradingLaboratory - Индикаторы")] //можно указать свою категорию

    //Название блока во вкладке "Подробно"

    [HelperName("BarNumberIfDifferentValue", Language = Constants.En)]
    [HelperName("Бар если отличается (Список)", Language = Constants.Ru)]
    [HandlerName("Бар если отличается (Список)")]

    //Описание во вкладке "Подробно" на руском и английском
    [Description("Выдаёт Список с номерами бара, встречавшихся до текущего, " +
        "значение индикатора на котором отличается от предыдущего " +
        "после обнаружение n-раз подряд (определяется параметром Steps)." +
        "Если не встречался ни разу, выдаёт 0. " +
        "Обсуждение в Telegram -  канал Лаборатория Трейдинга ( http://t.me/TradingLaboratory )")] //на русском

    //входящие параметры
    [InputsCount(1)] //количество входящих параметров


 //   [Input(1, TemplateTypes.INT, true, "Steps")]
    [Input(0, TemplateTypes.DOUBLE, true, "Indicator")]


    //результаты вычисления
    [OutputsCount(1)] //показывает количество  выдаваемых объектов
    [OutputType(TemplateTypes.DOUBLE)] //показывает тип выдаваемого объекта

    #endregion

    public class BarNumberIfDifferentList : 
        IDouble2DoubleHandler, //Создать список чисел из другого списка чисел
        IOneSourceHandler, //Обработчик с одним источником данных
        IStreamHandler, //Обработчик работает с потоками
        IHandler, //Базовый интерфейс обработчика
        IDoubleInputs, //Среди входящих чисел есть списки чисел
        IContextUses, //Обработчик использует данные кеша
        INeedVariableName, //Уникальное имя для кеширования
        IDoubleReturns //Будет возвращён список чисел
    {
        private TSLab.Script.Handlers.IContext context;

        private string m_variableName;

        #region указываем свойство, которое будет параметром обработчика


        [HandlerParameter(true, "2", Min = "1", Max = "100", Step = "1", Name ="Steps")]
        public int Steps
        {
            get;
            set;
        }


        #endregion

        public BarNumberIfDifferentList() //конструктор класса
        {
        }

        public TSLab.Script.Handlers.IContext Context
        {
            get
            {
                return this.context;
            }
            set
            {
                this.context = value;
            }
        }

        public string VariableName
        {
            get
            {
                return this.m_variableName;
            }
            set
            {
                this.m_variableName = value;
            }
        }

        public IList<double> Execute(IList<double> Indicator)
        {
            IList<double> barNumberList = new double[Indicator.Count];

            barNumberList[0] = 0; //присваиваем первому в списке значение 0

            for (int bar = 1; (bar < Indicator.Count); bar++)
            {
                barNumberList[bar] = barNumberList[bar-1];

                double counter = 0; //сбрасываем счётчик

                for (int j = bar; j > 1; j--)
                {

                    if (Indicator[j] != Indicator[j - 1]) //вводим искомое условие
                    {
                        counter = counter + 1;
                        if (counter >= Steps) //проверяем какой раз подряд встретилось искомое условие
                        {

                            barNumberList[bar] = j;
                            break; //выходим из внутреннего цикла и переходим к следующему бару
                        }

                    }
                    
                }
                
            }


                return barNumberList; //возвращаем искомый список чисел
                

        }
    }
}

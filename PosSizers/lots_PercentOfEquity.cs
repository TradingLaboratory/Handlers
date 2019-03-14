#region

using System;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options; //чтобы можно было указывать атрибуты класса "кубиков"
using System.ComponentModel; //чтобы работало "Описание

#endregion

namespace TSLab.TraidingLaboratory.Indicators
{


    #region параметры кубика "lots_PctOfEquity"

    //показывает в каком разделе кубиков будет находиться наш кубик
    [HandlerCategory("TradingLaboratory - Управление размером позиции")] //можно указать свою категорию

    //Название блока во вкладке "Подробно"
    [HelperName("lots_PctOfEquity", Language = Constants.Ru)] //на русском

    //Описание во вкладке "Подробно" на руском и английском
    [Description("Рассчитывает кол-во контрактов при использовании метода % от Эквити. Обсуждение в Telegram -  канал Лаборатория Трейдинга ( http://t.me/TradingLaboratory )")] //на русском

    //входящие параметры
    [InputsCount(2)] //количество входящих параметров

    [Input(0, TemplateTypes.DOUBLE, true, "money")]
    [Input(1, TemplateTypes.DOUBLE, true, "entryPrice")]
  //  [Input(2, TemplateTypes.DOUBLE, true, "pctOfEquity")]
    //    [TSLab.Script.Handlers.InputAttribute(0, TSLab.Script.Handlers.TemplateTypes.SECURITY, false, "symbol")]
    //    [TSLab.Script.Handlers.InputAttribute(4, TSLab.Script.Handlers.TemplateTypes.DOUBLE, true, "MaxPercentRisk")]


    //результаты вычисления
    [OutputsCount(1)] //показывает количество  выдаваемых объектов
    [OutputType(TemplateTypes.DOUBLE)] //показывает тип выдаваемого объекта

    #endregion

    public class lots_pctOfEquity :

        IDoubleReturns, //возвращает объект типа double
                        // ISecurityInput0, //в обработчике первый параметр - ценная бумага
        IValuesHandlerWithPrecalc //работает с текущими значениями, но требует перекалькуляции
                                  // System.IDisposable
    {
        #region указываем свойство, которое будет параметром обработчика
        [HandlerParameter(true, "100", Min = "10", Max = "1000", Step = "10", Name = "% от Equity")]
        public double pctOfEquity
        {
            get;
            set;
        }

        [HandlerParameter(true, "1", Min = "0", Max = "100", Step = "1", Name ="Акций в лоте")]
        public double LotSize
        {
            get;
            set;
        }

        [HandlerParameter(true, "1", Min = "1", Max = "100", Step = "0.1", Name ="Стоимость пункта, руб.")]
        public double punktPriceRUB //стоимость пункта цены (по умолчанию 1 - для РТС = 2 цента
                                    //стоимость шага цены делённая на шаг цены (по данным московской биржи)
        {
            get;
            set;
        }
        #endregion

        #region  объявляем переменные

        private double allMoneyForEnrty;
     //   private double lots;

        #endregion

        public lots_pctOfEquity() //конструктор класса - чтобы можно было создавать экземпляр класса в торговой стратегии
        {
        }

        #region предварительная перекалькуляция
        public void PreCalc()
        {

            //   this.symbol = symbol;
            allMoneyForEnrty = 0;
       //     lots = 0;

        }
        #endregion

        public double Execute(double money, double entryPrice, int i)
        {

            allMoneyForEnrty = money * pctOfEquity / 100.0; //в рублях

            double lots = allMoneyForEnrty / (entryPrice * LotSize * punktPriceRUB);

            if (lots < 1) lots = 0; //здесь можно подставить 1, чтобы даже если денег не хватает - всё равно входить одним контрактом

            lots = Math.Floor(lots);
         //   lots = symbol.RoundShares(lots);

            return lots;
            
        }


        #region пост Калькуляция
        public void PostCalc()
        {

        }

        #endregion

    }


}

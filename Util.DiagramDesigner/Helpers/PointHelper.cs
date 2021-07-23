using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Util.DiagramDesigner
{
    public class PointHelper
    {
        public static Point GetPointForConnector(FullyCreatedConnectorInfo connector)
        {
            Point point = new Point();
            if (connector.IsInnerPoint)
            {
                switch (connector.Orientation)
                {
                    case ConnectorOrientation.Top:
                        point = new Point(connector.DataItem.Left + connector.DataItem.ItemWidth * connector.XRatio + connector.ConnectorWidth / 2,
                                 connector.DataItem.Top + connector.DataItem.ItemHeight * connector.YRatio);
                        break;
                    case ConnectorOrientation.Bottom:
                        point = new Point(connector.DataItem.Left + connector.DataItem.ItemWidth * connector.XRatio + connector.ConnectorWidth / 2,
                                 connector.DataItem.Top + connector.DataItem.ItemHeight * connector.YRatio + connector.ConnectorHeight / 2); ;
                        break;
                    case ConnectorOrientation.Right:
                        point = new Point(connector.DataItem.Left + connector.DataItem.ItemWidth * connector.XRatio,
                                 connector.DataItem.Top + connector.DataItem.ItemHeight * connector.YRatio + connector.ConnectorHeight / 2);
                        break;
                    case ConnectorOrientation.Left:
                        point = new Point(connector.DataItem.Left + connector.DataItem.ItemWidth * connector.XRatio,
                                 connector.DataItem.Top + connector.DataItem.ItemHeight * connector.YRatio + connector.ConnectorHeight / 2);
                        break;
                    default:
                         point = new Point(connector.DataItem.Left + connector.DataItem.ItemWidth * connector.XRatio + connector.ConnectorWidth / 2,
                                 connector.DataItem.Top + connector.DataItem.ItemHeight * connector.YRatio + connector.ConnectorHeight / 2);
                        break;
                }

            }
            else
            {

                switch (connector.Orientation)
                {
                    case ConnectorOrientation.Top:
                        point = new Point(connector.DataItem.Left + (connector.DataItem.ItemWidth / 2), connector.DataItem.Top);
                        break;
                    case ConnectorOrientation.Bottom:
                        point = new Point(connector.DataItem.Left + (connector.DataItem.ItemWidth / 2), (connector.DataItem.Top + connector.DataItem.ItemHeight));
                        break;
                    case ConnectorOrientation.Right:
                        point = new Point(connector.DataItem.Left + connector.DataItem.ItemWidth, connector.DataItem.Top + (connector.DataItem.ItemHeight / 2));
                        break;
                    case ConnectorOrientation.Left:
                        point = new Point(connector.DataItem.Left, connector.DataItem.Top + (connector.DataItem.ItemHeight / 2));
                        break;
                }
            }
            return point;
        }


    }
}

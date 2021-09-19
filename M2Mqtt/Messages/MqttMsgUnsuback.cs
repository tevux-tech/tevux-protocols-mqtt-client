﻿/*
Copyright (c) 2013, 2014 Paolo Patierno

All rights reserved. This program and the accompanying materials
are made available under the terms of the Eclipse Public License v1.0
and Eclipse Distribution License v1.0 which accompany this distribution. 

The Eclipse Public License is available at 
   http://www.eclipse.org/legal/epl-v10.html
and the Eclipse Distribution License is available at 
   http://www.eclipse.org/org/documents/edl-v10.php.

Contributors:
   Paolo Patierno - initial API and implementation and/or initial documentation
*/

namespace uPLibrary.Networking.M2Mqtt.Messages {
    /// <summary>
    /// Class for UNSUBACK message from broker to client. See section 3.11.
    /// </summary>
    internal class MqttMsgUnsuback : MqttMsgBase {
        public MqttMsgUnsuback() {
            Type = MessageType.UnsubAck;
        }

        public override byte[] GetBytes() {
            // Not relevant for the client side, so just leaving it empty.
            return new byte[0];
        }

        public static bool TryParse(byte[] variableHeaderBytes, out MqttMsgUnsuback parsedMessage) {
            var isOk = true;
            parsedMessage = new MqttMsgUnsuback();

            // Bytes 1-2: Packet Identifier. Can be anything.
            parsedMessage.MessageId = (ushort)((variableHeaderBytes[0] << 8) + variableHeaderBytes[1]);

            return isOk;
        }

        public override string ToString() {
            return Helpers.GetTraceString("UNSUBACK", new object[] { "messageId" }, new object[] { MessageId });
        }
    }
}

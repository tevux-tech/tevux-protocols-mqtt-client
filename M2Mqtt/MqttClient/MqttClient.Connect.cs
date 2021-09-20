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

using System;
using System.Threading;

namespace Tevux.Protocols.Mqtt {
    public partial class MqttClient {
        /// <summary>
        /// Connect to broker
        /// </summary>
        /// <returns>Return code of CONNACK packet from broker</returns>
        public void Connect() {
            Connect(new ChannelConnectionOptions(), new MqttConnectionOptions());
        }

        public void Connect(ChannelConnectionOptions channelConnectionOptions) {
            Connect(channelConnectionOptions, new MqttConnectionOptions());
        }


        /// <summary>
        /// Connect to broker
        /// </summary>
        /// <returns>Return code of CONNACK packet from broker</returns>
        public void Connect(ChannelConnectionOptions channelConnectionOptions, MqttConnectionOptions mqttConnectionOptions) {
            if (_isInitialized == false) { throw new InvalidOperationException("MqttClient has not been initialized. Call Initialize() method first."); }

            ConnectionOptions = mqttConnectionOptions;

            var connectPacket = new ConnectPacket(mqttConnectionOptions);

            var isOk = true;

            if (_channel.TryConnect(channelConnectionOptions.Hostname, channelConnectionOptions.Port) == false) {
                isOk = false;
            };

            if (isOk) {
                LastCommTime = 0;
                _isConnectionClosing = false;
            }

            _connectStateMachine.Connect(connectPacket);
            while (_connectStateMachine.IsConnectionCompleted == false) {
                _connectStateMachine.Tick();
                Thread.Sleep(1000);
            }


            if (_connectStateMachine.ConnectionResult == ConnackPacket.ReturnCodes.Accepted) {
                _pingStateMachine.Reset();
                _connectStateMachine.Reset();

                // restore previous session
                RestoreSession();

                IsConnected = true;
            }

#warning needto use other constructs to return connection success. going for void temporarily.
            // return _connectStateMachine.ConnectionResult;
        }
    }
}

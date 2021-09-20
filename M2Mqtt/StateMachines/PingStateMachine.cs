﻿using Tevux.Protocols.Mqtt.Utility;

namespace Tevux.Protocols.Mqtt {
    internal class PingStateMachine {
        private bool _isWaitingForPingResponse;
        private double _requestTimestamp;
        private MqttClient _client;

        public bool IsBrokerAlive { get; private set; } = true;

        public void Initialize(MqttClient client) {
            _client = client;
            Reset();
        }

        public void Tick() {
            var currentTime = Helpers.GetCurrentTime();

            if (_isWaitingForPingResponse) {
                if (currentTime - _requestTimestamp > _client.ConnectionOptions.KeepAlivePeriod) {
                    // Problem. Server does not respond.
                    _isWaitingForPingResponse = false;
                    IsBrokerAlive = false;
                }
            }
            else {
                // Keep alive period equals zero means turning off keep alive mechanism.
                if ((currentTime - _client.LastCommTime > _client.ConnectionOptions.KeepAlivePeriod) && (_client.ConnectionOptions.KeepAlivePeriod > 0)) {
                    var pingreq = new PingreqPacket();
                    _client.Send(pingreq);
                    Trace.WriteLine(TraceLevel.Frame, $"{pingreq.GetShortName()}->");
                    _isWaitingForPingResponse = true;
                    _requestTimestamp = currentTime;
                }
            }
        }

        public void ProcessPacket(PingrespPacket packet) {
            Trace.WriteLine(TraceLevel.Frame, $"<-{packet.GetShortName()}");
            IsBrokerAlive = true;
            _isWaitingForPingResponse = false;
        }

        public void Reset() {
            IsBrokerAlive = true;
            _isWaitingForPingResponse = false;
        }
    }
}

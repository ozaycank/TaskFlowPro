import { useEffect } from 'react';

// This is a foundational hook. Later, it will connect to the actual SignalR HubConnection.
export const useSignalREvent = <T>(eventName: string, callback: (data: T) => void) => {
    useEffect(() => {
        // TO-DO: connection.on(eventName, callback)
        console.log(`[SignalR] Subscribed to ${eventName}`);

        return () => {
            // TO-DO: connection.off(eventName, callback)
            console.log(`[SignalR] Unsubscribed from ${eventName}`);
        };
    }, [eventName, callback]);
};
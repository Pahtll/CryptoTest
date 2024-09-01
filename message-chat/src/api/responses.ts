import { $host } from "./index";
import { TMessage } from "../types";

export async function get_messages(): Promise<TMessage[]> {
  const response = await $host.get("/messages/all");
  return response.data;
}

export async function create_message(message_text: string) {
  const response = await $host.post<string>(
    "/messages/create",
    message_text.toString(),
    {
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
  return response;
}

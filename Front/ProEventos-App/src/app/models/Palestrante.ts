import { Evento } from "./Evento";
import { UserUpdate } from "./identity/userUpdate";
import { RedeSocial } from "./RedeSocial";

export interface Palestrante {
  id: number;
  miniCurriculo: string;
  redesSociais: RedeSocial[];
  palestrantesEventos: Evento[];
  user: UserUpdate;
}

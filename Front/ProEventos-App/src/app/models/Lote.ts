import { Evento } from "./Evento";

export interface Lote {
  id: number;
  nomwe: string;
  preco: number;
  dataInicio?: Date;
  dataFim?: Date;
  quantidade: number;
  eventoId: number;
  evento: Evento;
}

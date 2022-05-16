{application, 'seshat', [
	{description, "Metrics library built for RabbitMQ, with a Prometheus exposition format built-in"},
	{vsn, "0.1.0"},
	{id, "v3.10.1"},
	{modules, ['seshat_app','seshat_counters','seshat_counters_server','seshat_sup']},
	{registered, [seshat_sup]},
	{applications, [kernel,stdlib,sasl,crypto]},
	{mod, {seshat_app, []}},
	{env, [
]}
]}.